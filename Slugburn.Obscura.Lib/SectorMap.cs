using System.Collections.Generic;

namespace Slugburn.Obscura.Lib
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
    }
}