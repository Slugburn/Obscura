using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib
{
    public class Player
    {
        public Player()
        {
            IdlePopulation = new ProductionQuantity();
            Sectors = new List<Sector>();
        }

        protected List<Sector> Sectors { get; set; }

        private IFaction _faction;

        public string Name { get; set; }

        public ShipBlueprint Interceptor { get; set; }

        public ShipBlueprint Cruiser { get; set; }

        public ShipBlueprint Dreadnaught { get; set; }

        public ShipBlueprint Starbase { get; set; }

        public int Money { get; set; }

        public int Science { get; set; }

        public int Materials { get; set; }

        public int ColonyShips { get; set; }

        public ProductionQuantity IdlePopulation { get; private set; }

        public PlayerColor Color { get { return _faction.Color; } }

        public string FactionName { get { return _faction.Name; } }

        public int HomeSectorId { get { return _faction.HomeSectorId; } }

        public void Setup(Game game)
        {
            Game = game;
            _faction = ChooseFaction(game);
            _faction.Setup(this);
            var startingLocation = ChooseStartingLocation();
            Sector homeSector = game.Sectors[HomeSectorId];
            startingLocation.Sector = homeSector;

            IdlePopulation[ProductionType.Money] = 11;
            IdlePopulation[ProductionType.Science] = 11;
            IdlePopulation[ProductionType.Material] = 11;

            homeSector.AddShip(Ship.FromBlueprint(Interceptor));

            ClaimSector(homeSector);

            foreach (var square in homeSector.Squares.Where(x=>!x.Advanced))
            {
                IdlePopulation[square.ProductionType]--;
                square.Owner = this;
            }
        }

        private void ClaimSector(Sector homeSector)
        {
            Sectors.Add(homeSector);
            homeSector.Owner = this;
            Influence--;
        }

        private IFaction ChooseFaction(Game game)
        {
            return game.GetAvailableFactions().Shuffle().Draw();
        }

        private MapLocation ChooseStartingLocation()
        {
            return Game.GetAvailableStartingLocations().Shuffle().Draw();
        }

        public Game Game { get; set; }

        public int Influence { get; set; }

        public bool HasFaction
        {
            get { return _faction != null; }
        }
    }

    public class ProductionQuantity
    {
        private readonly int[] _amount;

        public ProductionQuantity()
        {
            _amount=new int[4];
        }

        public int this[ProductionType type]
        {
            get { return _amount[(int) type - 1]; }
            set { _amount[(int) type - 1] = value; }
        }
    }
}
