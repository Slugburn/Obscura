using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib
{
    public class Game
    {
        public Game(IEnumerable<IFactionType> factionTypes, IEnumerable<IAction> actions)
        {
            _factionTypes = factionTypes;
            _actions = actions;
        }

        protected Game()
        {
        }

        private readonly IEnumerable<IFactionType> _factionTypes;
        private readonly IEnumerable<IAction> _actions;

        public void Setup(IList<Faction> factions)
        {
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

        public Faction StartingFaction { get; set; }

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

        public List<Faction> Factions { get; set; }

        protected List<Sector> OuterSectors { get; set; }

        protected List<Sector> MiddleSectors { get; set; }

        protected List<Sector> InnerSectors { get; set; }

        protected SectorMap Map { get; set; }

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

        protected int Round { get; set; }

        public IEnumerable<IFactionType> GetAvailableFactions()
        {
            var takenColors = Factions.Where(p=>p.HasFaction).Select(p=>p.Color);
            return _factionTypes.Where(f => !takenColors.Contains(f.Color));
        }

        public IEnumerable<MapLocation> GetAvailableStartingLocations()
        {
            return StartingLocations.Where(l => l.Sector == null);
        }

        public void StartTurn()
        {
            TakeAction(StartingFaction);
        }

        private void TakeAction(Faction faction)
        {
            faction.TakeAction(_actions, () => ActionDoneCallback(faction));
        }

        private void ActionDoneCallback(Faction faction)
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

        private Faction GetNextFaction(Faction faction)
        {
            var nextIndex = Factions.IndexOf(faction) + 1;
            if (nextIndex >= Factions.Count)
                nextIndex = 0;
            var nextFaction = Factions[nextIndex];
            return nextFaction;
        }

        private void StartCombatPhase()
        {
            // TODO: Combat phase
            StartUpkeepPhase();
        }

        private void StartUpkeepPhase()
        {
            var tasks = Factions.Select(faction=>Task.Factory.StartNew(faction.UpkeepPhase)).ToArray();
            Task.WaitAll(tasks);
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

    }
}
