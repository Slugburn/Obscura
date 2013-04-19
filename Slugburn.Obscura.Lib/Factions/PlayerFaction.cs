using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Combat;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Players;
using Slugburn.Obscura.Lib.Ships;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Factions
{
    public class PlayerFaction : IFaction
    {
        private readonly ILog _log;

        public PlayerFaction(ILog log, IPlayer player) : this()
        {
            _log = log;
            Player = player;
        }

        protected PlayerFaction()
        {
            IdlePopulation = new ProductionQuantity();
            Graveyard = new ProductionQuantity();
            Sectors = new List<Sector>();
            Ships = new List<PlayerShip>();
            Technologies = new List<Tech>();
            DiscoveredParts = new List<ShipPart>();
        }

        public override string ToString()
        {
            return Color.ToString();
        }

        public IEnumerable<Target> ChooseDamageDistribution(IEnumerable<DamageRoll> hits, IEnumerable<Target> targets)
        {
            return Player.ChooseDamageDistribution(hits, targets);
        }

        public List<Sector> Sectors { get; set; }

        private IFactionType _factionType;
        private int _buildCount;

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

        public FactionColor Color
        {
            get { return _factionType != null ? _factionType.Color : FactionColor.Undefined; }
        }

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
            _log.Log("\t{0} claims {1}", this, sector);
            Sectors.Add(sector);
            sector.Owner = this;
            Influence--;
            if (sector.DiscoveryTile == null) 
                return;
            
            var tile = sector.DiscoveryTile;
            _log.Log("{0} discovers {1}", this, tile);

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
            _log.Log("{0} chooses to {1}", this, chosenAction);
            if (!(chosenAction is PassAction))
            {
                Influence--;
                ActionsTaken++;
            }
            chosenAction.Do(this);
            Player.AfterAction(chosenAction);
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
            return UnknownTech().Where(tech=>CostFor(tech) <= Science);
        }

        public virtual IEnumerable<Tech> UnknownTech()
        {
            return Game.AvailableTechTiles.Except(Technologies);
        }

        public IList<Tech> Technologies { get; private set; }

        public int BuildCount
        {
            get { return Passed ? 1 : _buildCount; }
            set { _buildCount = value; }
        }

        public int UpgradeCount
        {
            get { return Passed ? 1 : 2; }
        }

        public IEnumerable<ShipBlueprint> Blueprints
        {
            get { return new[] {Interceptor, Cruiser, Dreadnought, Starbase}; }
        }

        public bool Passed { get; set; }

        public int MoveCount
        {
            get { return  Passed ? 1 : 3; }
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

        public Sector HomeSector { get { return Game.GetSectorById(HomeSectorId); } }

        public ProductionQuantity Graveyard { get; private set; }

        public List<ShipPart> DiscoveredParts { get; private set; }

        public virtual int CostFor(Tech tech)
        {
            var techDiscount = new[] {0, -1, -2, -3, -4, -6, -8, -8, -8};
            var discounted = tech.Cost + techDiscount[Technologies.Count(t => t.Category == tech.Category)];
            return discounted > tech.MinCost ? discounted : tech.MinCost;
        }

        public IEnumerable<ShipPart> GetAvailableShipParts()
        {
            return
                PartFactory.GetBasicParts()
                    .Concat(Technologies.Where(t => t is PartTech).Cast<PartTech>()
                                .Select(x => x.CreatePart()))
                    .Concat(DiscoveredParts);
        }

        public void UpkeepPhase()
        {
            Colonize();
            CivilizationUpkeep();
            Production();
            _log.Log("\t{0}: {1} Money, {2} Science, {3} Material", this, Money, Science, Material);
        }

        private void Production()
        {
            Science += GetProduction(ProductionType.Science);
            Material += GetProduction(ProductionType.Material);
        }

        public void Colonize()
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
                if ((productionType & square.ProductionType) != productionType)
                    throw new InvalidOperationException(String.Format("{0} square cannot produce {1}", square.ProductionType, productionType));
                ColonizePopulationSquare(square, productionType);
                ColonyShips--;
            }
        }

        private void ColonizePopulationSquare(PopulationSquare square, ProductionType productionType)
        {
            square.Owner = this;
            IdlePopulation[productionType]--;
            _log.Log("{0} colonizes {1} planet in {2} to produce {3}", this, square, square.Sector, productionType);
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
            if (influence < 0 || influence > 16)
                throw new Exception(String.Format("Influence {0} is not valid.", influence));
            var influenceTrack = new[] {-30, -25, -21, -17, -13, -10, -7, -5, -3, -2, -1, 0, 0, 0, 0, 0, 0};
            return influenceTrack[influence];
        }

        public void CleanupPhase()
        {
            Passed = false;
            Influence += ActionsTaken;
            ActionsTaken = 0;
            foreach (var prodType in new[]{ProductionType.Material, ProductionType.Material, ProductionType.Science})
            {
                IdlePopulation[prodType] += Graveyard[prodType];
                Graveyard[prodType] = 0;
            }
            ColonyShips = MaxColonyShips;
        }

        public bool HasTechnology(Tech tech)
        {
            return Technologies.Any(x=> Equals(x, tech));
        }

        public int GetIncomeForInfluence(int influence)
        {
            return Money + GetProduction(ProductionType.Money) + GetUpkeep(influence);
        }

        public Sector GetClosestSectorTo(Sector sector)
        {
            if (sector.Owner == this)
                return sector;
            var myAdjacent = sector.AdjacentSectors().Where(x => x.Owner == this).ToList();
            if (myAdjacent.Count > 0)
                return myAdjacent.PickRandom();
            var closest = (from x in Sectors
                          let path = GetShortestPath(x, sector)
                          where path != null
                          orderby path.Count
                          select x).FirstOrDefault();
            return closest;
        }

        public Sector[] GetInfluencePlacementLocations()
        {
            // valid placement locations include any unclaimed sectors adjacent to my sectors or ships
            // that do not have enemy ships
            return Sectors.SelectMany(x => x.AdjacentSectors())
                          .Concat(Ships.SelectMany(x => x.Sector.AdjacentSectors().Concat(new [] {x.Sector})))
                          .Distinct()
                          .Where(x => x.Owner == null && !x.GetEnemyShips(this).Any())
                          .ToArray();
        }

        public void Trade(ProductionType trade, ProductionType tradeFor)
        {
            if (trade == ProductionType.Material)
                Material -= TradeRatio;
            if (trade == ProductionType.Money)
                Money -= TradeRatio;
            if (trade == ProductionType.Science)
                Science -= TradeRatio;
            if (tradeFor == ProductionType.Material)
                Material += 1;
            if (tradeFor == ProductionType.Money)
                Money += 1;
            if (tradeFor == ProductionType.Science)
                Science += 1;
        }

        private static IList<Sector> GetShortestPath(PlayerFaction faction, Sector start, Sector destination, IEnumerable<Sector> path, ref int shortest)
        {
            path = path.Concat(new[]{start}).ToArray();
            if (path.Count() > 10)
                return null;
            if (path.Count() >= shortest)
                return null;
            if (start == destination)
            {
                shortest = path.Count();
                return path.ToList();
            }
            IEnumerable<Sector> thePath = null;
            foreach (var adjacent in start.AdjacentSectors().Where(s=>s==destination || !s.GetEnemyShips(faction).Any()).Except(path))
            {
                var adjPath = GetShortestPath(faction, adjacent, destination, path, ref shortest);
                if (adjPath != null && adjPath.Count <= shortest)
                    thePath = adjPath;
            }
            return thePath != null ? thePath.ToList()  : new List<Sector>();
        }

        internal IList<Sector> GetShortestPath(Sector start, Sector destination)
        {
            if (start==destination)
                return new Sector[0];
            var shortest = Int32.MaxValue;
            var shortestPath = GetShortestPath(this, start, destination, new Sector[0], ref shortest).Skip(1).ToList();
            return shortestPath.Count > 0 ? shortestPath : null;
        }

        public void RelinquishSector(Sector sector)
        {
            sector.Owner = null;
            Sectors.Remove(sector);
            Influence++;
        }

        public void AbandonSector(Sector sector)
        {
            _log.Log("{0} abandons {1}", this, sector);
            foreach (var square in sector.Squares.Where(x => x.Owner != null))
            {
                square.Owner = null;
                var prodType = square.ProductionType;
                if (prodType==ProductionType.Orbital || prodType==ProductionType.Any)
                    prodType = Player.ChooseProductionToAbandon(prodType);
                IdlePopulation[prodType]++;
            }
            RelinquishSector(sector);
        }

        public void Log(string messageFormat, params object[] args)
        {
            _log.Log(messageFormat, args);
        }
    }
}
