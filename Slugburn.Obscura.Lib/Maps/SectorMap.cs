using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Obscura.Lib.Maps
{
    public class SectorMap
    {
        private readonly Dictionary<MapCoord, Sector> _map;

        public SectorMap()
        {
            _map = new Dictionary<MapCoord, Sector>();
        }

        public MapCoord Coord(int x, int y)
        {
            return new MapCoord(x,y);
        }

        public void Place(Sector sector, MapCoord coord)
        {
            _map[coord] = sector;
            sector.Location = new MapLocation(this, coord);
        }

        public MapLocation[] GetStartingLayout(int playerCount)
        {
            var center = new MapCoord(0,0);
            var twelve = center.Go(12, 2);
            var two = center.Go(2, 2);
            var four = center.Go(4, 2);
            var six = center.Go(6, 2);
            var eight = center.Go(8, 2);
            var ten = center.Go(10, 2);
            MapCoord[] coords;
            switch (playerCount)
            {
                case 2:
                    coords = new[] {twelve, six};
                    break;
                case 3:
                    coords = new[] {twelve, four, eight};
                    break;
                case 4:
                    coords = new[] {two, four, eight, ten};
                    break;
                case 5:
                    coords = new[] {twelve, two, four, eight, ten};
                    break;
                case 6:
                    coords = new[] {twelve, two, four, six, eight, ten};
                    break;
                default:
                    throw new ArgumentOutOfRangeException("playerCount");
            }
            return coords.Select(c => new MapLocation(this, c)).ToArray();
        }

        public Sector GetSector(MapCoord coord)
        {
            return _map.ContainsKey(coord) ? _map[coord] : null;
        }
    }
}