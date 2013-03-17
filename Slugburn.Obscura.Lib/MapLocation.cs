namespace Slugburn.Obscura.Lib
{
    public class MapLocation
    {
        public SectorMap Map { get; set; }
        public MapCoord Coord { get; set; }

        public Sector Sector
        {
            get { return Map.GetSector(Coord); }
            set { Map.Place(value, Coord); }
        }

        public MapLocation(SectorMap map, MapCoord coord)
        {
            Map = map;
            Coord = coord;
        }
    }
}