using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Players;
using Slugburn.Obscura.Lib.Ships;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Factions
{
    public class Faction
    {
        public Faction()
        {
            IdlePopulation = new ProductionQuantity();
            Sectors = new List<Sector>();
            Ships = new List<PlayerShip>();
            Technologies = new List<Tech>();
            Player = new RandomPlayer();
        }

        public List<Sector> Sectors { get; set; }

        private IFactionType _factionType;

        public string Name { get; set; }

        public ShipBlueprint Interceptor { get; set; }

        public ShipBlueprint Cruiser { get; set; }

        public ShipBlueprint Dreadnought { get; set; }

        public ShipBlueprint Starbase { get; set; }

        public int Money { get; set; }

        public int Science { get; set; }

        public int Materials { get; set; }

        public int ColonyShips { get; set; }

        public ProductionQuantity IdlePopulation { get; private set; }

        public FactionColor Color { get { return _factionType.Color; } }

        public string FactionName { get { return _factionType.Name; } }

        public int HomeSectorId { get { return _factionType.HomeSectorId; } }

        public void Setup(Game game)
        {
            Game = game;
            _factionType = Player.ChooseFaction(game.GetAvailableFactions());
            _factionType.Setup(this);
            var startingLocation = Player.ChooseStartingLocation(Game.GetAvailableStartingLocations());
            var homeSector = game.Sectors[HomeSectorId];
            startingLocation.Sector = homeSector;

            IdlePopulation[ProductionType.Money] = 11;
            IdlePopulation[ProductionType.Science] = 11;
            IdlePopulation[ProductionType.Material] = 11;

            var interceptor = CreateShip(Interceptor);
            homeSector.AddShip(interceptor);

            ClaimSector(homeSector);

            foreach (var square in homeSector.Squares.Where(x=>!x.Advanced))
            {
                IdlePopulation[square.ProductionType]--;
                square.Owner = this;
            }
        }

        public PlayerShip CreateShip(ShipBlueprint blueprint)
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
                if (Player.ChooseToUseDiscovery(tile))
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
            get { return _factionType != null; }
        }

        public List<PlayerShip> Ships { get; set; }

        public virtual IPlayer Player { get; set; }

        public void TakeAction(IEnumerable<IAction> actions, Action callback)
        {
            var validActions = actions.Where(action => action.IsValid(this));
            var chosenAction = Player.ChooseAction(validActions);
            chosenAction.Do(this);
            if (!(chosenAction is PassAction))
            {
                Influence--;
                ActionsTaken++;
            }
            callback();
        }

        protected int ActionsTaken { get; set; }

        public virtual IEnumerable<MapLocation> GetValidExplorationLocations()
        {
            var sourceSectors = Sectors.Concat(Ships.Where(ship=>!ship.IsPinned).Select(ship => ship.Sector)).Distinct();
            return sourceSectors.SelectMany(x => x.Location.AdjacentExplorable()).Distinct();
        }

        public virtual IEnumerable<Tech> AvailableResearchTech()
        {
            return Game.AvailableTechTiles.Where(tech=>CostFor(tech) <= Science && !Technologies.Contains(tech));
        }

        public IList<Tech> Technologies { get; private set; }

        public int BuildCount
        {
            get { return 2; }
        }

        public int UpgradeCount
        {
            get { return 2; }
        }

        public IEnumerable<ShipBlueprint> Blueprints
        {
            get { return new[] {Interceptor, Cruiser, Dreadnought, Starbase}; }
        }

        public bool Passed { get; set; }

        public virtual int CostFor(Tech tech)
        {
            var techDiscount = new[] {0, -1, -2, -3, -4, -6, -8};
            var discounted = tech.Cost + techDiscount[Technologies.Count(t => t.Category == tech.Category)];
            return discounted > tech.MinCost ? discounted : tech.MinCost;
        }

        public IEnumerable<ShipPart> GetAvailableShipParts()
        {
            return PartFactory.GetBasicParts().Concat(Technologies.Where(t => t is PartTech).Cast<PartTech>().Select(x => x.CreatePart()));
        }

        public void UpkeepPhase()
        {
            Colonize();
            CivilizationUpkeep();
        }

        private void Colonize()
        {
            while (ColonyShips > 0)
            {
                var opportunities = Sectors.SelectMany(x => x.Squares.Where(s => s.Owner == null && CanColonize(s))).ToList();
                if (opportunities.Count == 0)
                    break;
                var square = Player.ChooseColonizationLocation(opportunities);
                if (square == null)
                    break;
                square.Owner = this;
                IdlePopulation[square.ProductionType]--;
            }
        }

        private bool CanColonize(PopulationSquare square)
        {
            return !square.Advanced;
        }

        private void CivilizationUpkeep()
        {
            throw new NotImplementedException();
        }

    }
}
