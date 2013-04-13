using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Players;
using Slugburn.Obscura.Lib.Ships;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Factions
{
    public class Faction
    {
        private readonly ILog _log;

        public Faction(ILog log, IList<IBuilder> builders) : this()
        {
            _log = log;
            Player = new RandomPlayer(new BlueprintGenerator(), builders);
        }

        protected Faction()
        {
            IdlePopulation = new ProductionQuantity();
            Sectors = new List<Sector>();
            Ships = new List<PlayerShip>();
            Technologies = new List<Tech>();
        }

        public List<Sector> Sectors { get; set; }

        private IFactionType _factionType;

        public string Name { get { return _factionType != null ? _factionType.Name : null; } }

        public ShipBlueprint Interceptor { get; set; }

        public ShipBlueprint Cruiser { get; set; }

        public ShipBlueprint Dreadnought { get; set; }

        public ShipBlueprint Starbase { get; set; }

        public int Money { get; set; }

        public int Science { get; set; }

        public int Material { get; set; }

        public int ColonyShips { get; set; }

        public ProductionQuantity IdlePopulation { get; private set; }

        public FactionColor Color { get { return _factionType.Color; } }

        public int HomeSectorId { get { return _factionType.HomeSectorId; } }

        public void Setup(Game game)
        {
            Game = game;
            Player.Faction = this;
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
            _log.Log("{0} claims {1} ({2})", Name, sector.Name, sector.Id);
            Sectors.Add(sector);
            sector.Owner = this;
            Influence--;
            if (sector.HasDiscovery )
            {
                var tile = sector.DiscoveryTile;
                _log.Log("{0} discovers {1}", Name, tile.Name);

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

        public void TakeAction(IEnumerable<IAction> actions)
        {
            var validActions = actions.Where(action => action.IsValid(this));
            var chosenAction = Player.ChooseAction(validActions);
            _log.Log("{0} chooses to {1}", Name, chosenAction.Name);
            if (!(chosenAction is PassAction))
            {
                Influence--;
                ActionsTaken++;
            }
            chosenAction.Do(this);
        }

        protected int ActionsTaken { get; set; }

        public virtual IEnumerable<MapLocation> GetValidExplorationLocations()
        {
            var sourceSectors = Sectors.Concat(Ships.Where(ship=>!ship.IsPinned).Select(ship => ship.Sector)).Distinct();
            var explorableLocations = sourceSectors.SelectMany(x => x.Location.AdjacentExplorable()).Distinct().ToList();
            if (Game.OuterSectors.Count==0)
            {
                var outerLocations = explorableLocations.Where(x => x.DistanceFromCenter >= 3);
                explorableLocations = explorableLocations.Except(outerLocations).ToList();
            }
            return explorableLocations;
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

        public decimal MoveCount
        {
            get { return 3; }
        }

        public int MaxColonyShips { get; set; }

        public int TradeRatio
        {
            get { return 2; }
        }

        public int OrbitalCost
        {
            get { return 5; }
        }

        public int MonolithCost
        {
            get { return 10; }
        }

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
            Production();
            _log.Log("After upkeep, {0} has {1} Money, {2} Science, and {3} Material", Name, Money, Science, Material);
        }

        private void Production()
        {
            Science += GetProduction(ProductionType.Science);
            Material += GetProduction(ProductionType.Material);
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
                var productionType = square.ProductionType;
                if (productionType == ProductionType.Orbital || productionType == ProductionType.Any)
                    productionType = Player.ChooseColonizationType(productionType);
                ColonizePopulationSquare(square, productionType);
            }
        }

        private void ColonizePopulationSquare(PopulationSquare square, ProductionType productionType)
        {
            square.Owner = this;
            IdlePopulation[productionType]--;
            _log.Log("{0} colonizes {1} planet in {2} to produce {3}", Name, square, square.Sector, productionType);
        }

        private bool CanColonize(PopulationSquare square)
        {
            if (!square.Advanced)
                return true;
            var advancedTech = new Dictionary<ProductionType, Tech>
                {
                    {ProductionType.Money, Tech.AdvancedEconomy},
                    {ProductionType.Science, Tech.AdvancedLabs},
                    {ProductionType.Material, Tech.AdvancedMining}
                };
            var requiredTech = advancedTech[square.ProductionType];
            return Technologies.Any(tech => tech.Equals(requiredTech));
        }

        private void CivilizationUpkeep()
        {
            Money += GetProduction(ProductionType.Money);
            while (Money + GetUpkeep(Influence) <  0)
                Player.HandleBankruptcy();
            Money += GetUpkeep(Influence);
        }

        public int GetProduction(ProductionType productionType)
        {
            var productionTrack = new[] {28, 24, 21, 18, 15, 12, 10, 8, 6, 4, 3, 2};
            return productionTrack[IdlePopulation[productionType]];
        }

        public static int GetUpkeep(int influence)
        {
            if (influence < 0 || influence > 15)
                throw new Exception(String.Format("Influcent {0} is not valid.", influence));
            var influenceTrack = new[] {-30, -25, -21, -17, -13, -10, -7, -5, -3, -2, -1, 0, 0, 0, 0, 0};
            return influenceTrack[influence];
        }

        public void CleanupPhase()
        {
            Passed = false;
            Influence += ActionsTaken;
            ActionsTaken = 0;
            ColonyShips = MaxColonyShips;
        }

        public bool HasTechnology(Tech tech)
        {
            return Technologies.Any(x=> Equals(x, tech));
        }

        public bool SpendingInfluenceWillBankrupt()
        {
            return Money + GetProduction(ProductionType.Money) + GetUpkeep(Influence - 1) < 0;
        }

        public double CombatSuccessRatio(Sector mySector, IEnumerable<Sector> enemySectors)
        {
            var friendlyRating = mySector.GetFriendlyShips(this).GetTotalRating();
            var enemyRating = enemySectors.SelectMany(sector => sector.GetEnemyShips(this)).GetTotalRating();
            var ratio = friendlyRating/enemyRating;
            return double.IsInfinity(ratio) ? 100 : ratio;
        }

        public double CombatSuccessRatio(Sector sector)
        {
            return CombatSuccessRatio(sector, new[] {sector});
        }
    }
}
