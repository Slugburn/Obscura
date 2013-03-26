﻿using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib
{
    public class Game
    {
        private IList<IFactionType> _factions;

        public void Setup(IList<Faction> players)
        {
            Players = players.Shuffle();
            StartingFaction = Players[0];
            _factions = FactionTypeCatalog.GetFactionTypes();
            Round = 1;
            TechTiles = TechCatalog.GetTiles().Shuffle();
            AvailableTechTiles = TechTiles.Draw(GetStartingTechCount(Players.Count));
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
            OuterSectors = Sectors.Values.Where(s => s.IsOuter).Shuffle().Draw(GetOuterSectorCount(Players.Count));
            StartingLocations = Map.GetStartingLayout(Players.Count);

            players.Each(p=>p.Setup(this));
        }

        protected Faction StartingFaction { get; set; }

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

        protected List<Faction> Players { get; set; }

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
            var takenColors = Players.Where(p=>p.HasFaction).Select(p=>p.Color);
            return _factions.Where(f => !takenColors.Contains(f.Color));
        }

        public IEnumerable<MapLocation> GetAvailableStartingLocations()
        {
            return StartingLocations.Where(l => l.Sector == null);
        }

        public void Start()
        {
            StartingFaction.TakeAction();
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
