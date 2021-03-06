using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Combat;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Messages;
using Slugburn.Obscura.Lib.Messaging;
using Slugburn.Obscura.Lib.Players;
using Slugburn.Obscura.Lib.Ships;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Factions
{
    public class Faction : IShipOwner
    {
        private readonly ILog _log;
        private readonly IEnumerable<IMessageHandler<Faction>> _messageHandlers;
        private readonly IEnumerable<IAction> _actions;

        public Faction(
            IMessagePipe messagePipe,
            ILog log, 
            IPlayer player, 
            IEnumerable<IMessageHandler<Faction>> messageHandlers,
            IEnumerable<IAction> actions
            ) : this()
        {
            _log = log;
            _messageHandlers = messageHandlers;
            _actions = actions;
            MessagePipe = messagePipe;
            Player = player;
            _messageHandlers.Configure(this, MessagePipe);
        }

        protected Faction()
        {
            Influence = 13;
            IdlePopulation = new ProductionQuantity();
            Graveyard = new ProductionQuantity();
            Sectors = new List<Sector>();
            Ships = new List<PlayerShip>();
            Technologies = new List<Tech>();
            DiscoveredParts = new List<ShipPart>();
            Interceptor = new ShipBlueprint();
            Cruiser = new ShipBlueprint();
            Dreadnought = new ShipBlueprint();
            Starbase = new ShipBlueprint();
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
        private int _influence;

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
            Player.SetFaction(this);

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

            InfluenceSector(homeSector);

            foreach (var square in homeSector.Squares.Where(x=>!x.Advanced))
            {
                ColonizePopulationSquare(square, square.ProductionType);
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
            InfluenceSector(sector);
            if (sector.DiscoveryTile != null)
                ClaimDiscoveryTile(sector);
            SendMessage(new SectorClaimed(sector));
        }

        private void InfluenceSector(Sector sector)
        {
            _log.Log("\t{0} claims {1}", this, sector);
            Sectors.Add(sector);
            sector.Owner = this;
            Influence--;
        }

        private void ClaimDiscoveryTile(Sector sector)
        {
            var tile = sector.DiscoveryTile;
            _log.Log("\t{0} discovers {1}", this, tile);

            sector.DiscoveryTile = null;

            if (Player.ChooseToUseDiscovery(tile))
            {
                tile.Use(new DiscoveryUsage(this, sector));
            }
            else
            {
                this.Vp += 2;
            }
        }

        protected int Vp { get; set; }

        public Game Game { get; set; }

        public int Influence
        {
            get { return _influence; }
            set
            {
                if (value < 0 || value > 16)
                    throw new ArgumentOutOfRangeException("Influence", value, String.Format("Influence {0} is not valid.", value));
                _influence = value;
            }
        }

        public bool HasFaction
        {
            get { return _factionType != null; }
        }

        public List<PlayerShip> Ships { get; set; }

        public virtual IPlayer Player { get; set; }

        public void TakeAction()
        {
            var validActions = _actions.Where(action => action.IsValid(this));
            var chosenAction = Player.ChooseAction(validActions);
            _log.Log("{0} chooses to {1}", this, chosenAction);
            if (!(chosenAction is PassAction))
            {
                Influence--;
                ActionsTaken++;
            }
            chosenAction.Do(this);
            SendMessage(new ActionComplete(chosenAction));
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

        public IMessagePipe MessagePipe { get; private set; }

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
                if (opportunities.All(x=>square != x))
                    throw new InvalidOperationException(string.Format("{0} can not colonize {1} square in {2}", this, square, square.Owner));
                var productionType = square.ProductionType;
                if (productionType == ProductionType.Orbital || productionType == ProductionType.Any)
                    productionType = Player.ChooseColonizationType(productionType);
                if ((productionType & square.ProductionType) != productionType)
                    throw new InvalidOperationException(String.Format("{0} square cannot produce {1}", square.ProductionType, productionType));
                ColonizePopulationSquare(square, productionType);
                ColonyShips--;
                Game.SendMessage(new SectorUpdated(square.Sector));
            }
        }

        private void ColonizePopulationSquare(PopulationSquare square, ProductionType productionType)
        {
            square.Owner = this;
            IdlePopulation[productionType]--;
            if (productionType != square.ProductionType)
                _log.Log("\t{0} colonizes {1} planet in {2} to produce {3}", this, square, square.Sector, productionType);
            else
                _log.Log("\t{0} colonizes {1} planet in {2}", this, square, square.Sector);
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
            if (influence < 0)
                return int.MaxValue;
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
            _log.Log("\t*{0} trades {1} {2} for 1 {3}", this, TradeRatio, trade, tradeFor);
        }

        private static IList<Sector> GetShortestPath(Faction faction, Sector start, Sector destination, IEnumerable<Sector> path, ref int shortest)
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

        public virtual void SendMessage<T>(T message)
        {
            MessagePipe.Publish(message);
        }

        public T GetAction<T>() where T: IAction
        {
            return (T) _actions.SingleOrDefault(x => x is T);
        }
    }
}
