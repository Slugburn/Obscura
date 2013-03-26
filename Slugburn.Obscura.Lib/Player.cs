using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Controllers;
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
            Ships = new List<PlayerShip>();
            Controller = new RandomController();
        }

        public List<Sector> Sectors { get; set; }

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
            _faction = Controller.ChooseFaction(game.GetAvailableFactions());
            _faction.Setup(this);
            var startingLocation = Controller.ChooseStartingLocation(Game.GetAvailableStartingLocations());
            var homeSector = game.Sectors[HomeSectorId];
            startingLocation.Sector = homeSector;

            IdlePopulation[ProductionType.Money] = 11;
            IdlePopulation[ProductionType.Science] = 11;
            IdlePopulation[ProductionType.Material] = 11;

            var interceptor = CreateShip(Interceptor);
            interceptor.SetSector(homeSector);

            ClaimSector(homeSector);

            foreach (var square in homeSector.Squares.Where(x=>!x.Advanced))
            {
                IdlePopulation[square.ProductionType]--;
                square.Owner = this;
            }
        }

        private PlayerShip CreateShip(ShipBlueprint blueprint)
        {
            var ship = new PlayerShip(this, blueprint);
            Ships.Add(ship);
            return ship;
        }

        public virtual void ClaimSector(Sector sector)
        {
            Sectors.Add(sector);
            sector.Owner = this;
            Influence--;
            if (sector.HasDiscovery )
            {
                var tile = sector.DiscoveryTile;
                sector.DiscoveryTile = null;
                if (Controller.ChooseToUseDiscovery(tile))
                {
                    tile.Use(this);
                }
                else
                {
                    this.Vp += 2;
                }
            }
        }

        protected int Vp { get; set; }

        public Game Game { get; set; }

        public int Influence { get; set; }

        public bool HasFaction
        {
            get { return _faction != null; }
        }

        public List<PlayerShip> Ships { get; set; }

        public virtual IPlayerController Controller { get; set; }

        public void TakeAction()
        {
            var validActions = ActionCatalog.All.Where(action => action.IsValid(this));
            var chosenAction = Controller.ChooseAction(validActions);
            chosenAction.Do(this);
        }

        public virtual IEnumerable<MapLocation> GetValidExplorationLocations()
        {
            var sourceSectors = Sectors.Concat(Ships.Where(ship=>!ship.IsPinned).Select(ship => ship.Sector)).Distinct();
            return sourceSectors.SelectMany(x => x.Location.AdjacentExplorable()).Distinct();
        }
    }
}
