using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Combat;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib
{
    public class Game
    {
        public Game(IEnumerable<IFactionType> factionTypes, IEnumerable<IAction> actions, ILog log, Random random, CombatEngine combatEngine)
        {
            _factionTypes = factionTypes;
            _actions = actions;
            _log = log;
            _random = random;
            _combatEngine = combatEngine;
            Ancients = new AncientFaction();
        }

        public AncientFaction Ancients { get; private set; }

        protected Game()
        {
        }

        private readonly IEnumerable<IFactionType> _factionTypes;
        private readonly IEnumerable<IAction> _actions;
        private readonly ILog _log;
        private readonly Random _random;
        private readonly CombatEngine _combatEngine;

        public void Setup(IList<PlayerFaction> factions)
        {
            _log.Log("-- Setup --");
            Factions = factions.Shuffle();
            StartingFaction = Factions[0];
            Round = 1;
            TechTiles = TechCatalog.GetTiles().Shuffle();
            AvailableTechTiles = TechTiles.Draw(GetStartingTechCount(Factions.Count));
            ReputationTiles = GetRepTiles().Shuffle();
            DiscoveryTiles = DiscoveryCatalog.GetTiles().Shuffle();
            Sectors = SectorCatalog.GetSectors();
            Map = new SectorMap();
            var galacticCenter = Sectors[1];
            Map.Place(galacticCenter, Map.Coord(0, 0));
            galacticCenter.DiscoveryTile = DiscoveryTiles.Draw();
            var gcds = new GalacticCenterDefenseSystem();
            galacticCenter.AddShip(gcds);
            InnerSectors = Sectors.Values.Where(s => s.IsInner).Shuffle();
            MiddleSectors = Sectors.Values.Where(s => s.IsMiddle).Shuffle();
            OuterSectors = Sectors.Values.Where(s => s.IsOuter).Shuffle().Draw(GetOuterSectorCount(Factions.Count));
            StartingLocations = Map.GetStartingLayout(Factions.Count);

            factions.Each(p=>p.Setup(this));
        }

        public PlayerFaction StartingFaction { get; set; }

        protected MapLocation[] StartingLocations { get; set; }

        private static int GetOuterSectorCount(int playerCount)
        {
            var count = new Dictionary<int, int>
                            {
                                {2, 5},
                                {3, 10},
                                {4, 14},
                                {5, 16},
                                {6, 18}
                            };
            return count[playerCount];
        }

        private static int GetStartingTechCount(int playerCount)
        {
            return 8 + playerCount * 2;
        }

        public List<PlayerFaction> Factions { get; set; }

        public List<Sector> OuterSectors { get; set; }

        protected List<Sector> MiddleSectors { get; set; }

        protected List<Sector> InnerSectors { get; set; }

        public SectorMap Map { get; set; }

        public Dictionary<int, Sector> Sectors { get; set; }

        public List<Discovery> DiscoveryTiles { get; set; }

        protected List<int> ReputationTiles { get; set; }

        private static IEnumerable<int> GetRepTiles()
        {
            // 12 x 1, 9 x 2, 7 x 3, 4 x 4
            return Enumerable.Repeat(1, 12)
                .Concat(Enumerable.Repeat(2, 9))
                .Concat(Enumerable.Repeat(3, 7)
                .Concat(Enumerable.Repeat(4, 4)));
        }

        public List<Tech> AvailableTechTiles { get; set; }

        public IList<Tech> TechTiles { get; set; }

        public int Round { get; set; }

        public Sector GalacticCore
        {
            get { return Sectors[1]; }
        }

        public IEnumerable<IFactionType> GetAvailableFactions()
        {
            var takenColors = Factions.Where(p=>p.HasFaction).Select(p=>p.Color);
            return _factionTypes.Where(f => !takenColors.Contains(f.Color));
        }

        public IEnumerable<MapLocation> GetAvailableStartingLocations()
        {
            return StartingLocations.Where(l => l.Sector == null);
        }

        public void Start()
        {
            try
            {
                while (Round <= 10)
                {
                    _log.Log("-- Round {0} --", Round);
                    var currentFaction = StartingFaction;
                    while (Factions.Any(f=>!f.Passed))
                    {
                        currentFaction.TakeAction(_actions);
                        currentFaction = GetNextFaction(currentFaction);
                    }
                    StartCombatPhase();
                    StartUpkeepPhase();
                    StartCleanupPhase();
                }
            }
            finally
            {
                new MapVisualizer(_log).Display(Map);
            }
        }

        private void TakeAction(PlayerFaction faction)
        {
            faction.TakeAction(_actions);
            ActionDone(faction);
        }

        private void ActionDone(PlayerFaction faction)
        {
            // Have all players passed?
            if (Factions.All(f => f.Passed))
            {
                StartCombatPhase();
                return;
            }
            var nextFaction = GetNextFaction(faction);
            TakeAction(nextFaction);
        }

        private PlayerFaction GetNextFaction(PlayerFaction faction)
        {
            var nextIndex = Factions.IndexOf(faction) + 1;
            if (nextIndex >= Factions.Count)
                nextIndex = 0;
            var nextFaction = Factions[nextIndex];
            return nextFaction;
        }

        private void StartCombatPhase()
        {
            _combatEngine.ResolveCombatPhase(Map.GetSectors().ToArray());
        }

        private void StartUpkeepPhase()
        {
            _log.Log("- Upkeep -");
            var tasks = Factions.Select(faction=>Task.Factory.StartNew(faction.UpkeepPhase)).ToArray();
            Task.WaitAll(tasks);
        }

        private void StartCleanupPhase()
        {
            var newTech = TechTiles.Draw(NewTechCount());
            AvailableTechTiles.AddRange(newTech);
            var tasks = Factions.Select(faction => Task.Factory.StartNew(faction.CleanupPhase)).ToArray();
            Task.WaitAll(tasks);
            Round++;
        }

        private int NewTechCount()
        {
            var count = new Dictionary<int, int>
                            {
                                {2, 5},
                                {3, 10},
                                {4, 14},
                                {5, 16},
                                {6, 18}
                            };
            return count[Factions.Count];
        }

        public virtual Sector GetSectorFor(MapLocation location)
        {
            switch (location.DistanceFromCenter)
            {
                case 1:
                    return InnerSectors.Draw();
                case 2:
                    return MiddleSectors.Draw();
                default:
                    return OuterSectors.Draw();
            }
        }

        public Sector GetSectorById(int sectorId)
        {
            return Sectors[sectorId];
        }
    }

}
