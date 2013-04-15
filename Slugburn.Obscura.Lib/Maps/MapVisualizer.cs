using System.Linq;

namespace Slugburn.Obscura.Lib.Maps
{
    public class MapVisualizer
    {
        private readonly ILog _log;

        public MapVisualizer(ILog log)
        {
            _log = log;
        }

        public void Display(SectorMap map)
        {
            var sectors = map.GetSectors().ToArray();
            var minX = sectors.Min(s => s.Location.Coord.X);
            var minY = sectors.Min(s => s.Location.Coord.Y);
            var maxX = sectors.Max(s => s.Location.Coord.X);
            var maxY = sectors.Max(s => s.Location.Coord.Y);
            for (var y = maxY;y>=minY;y--)
            {
                for (var row = 0;row<6;row++)
                {
                    WriteRow(map, y, minY, row, minX, maxX);
                }
            }
        }

        private void WriteRow(SectorMap map, int y, int minY, int row, int minX, int maxX)
        {
            var line = "".PadRight((y-minY)*5);
            for (var x = maxX; x >= minX; x--)
            {
                var sector = map.GetSector(new MapCoord(x,y));
                if (sector == null)
                    line += "          ";
                else
                {
                    switch (row)
                    {
                        case 1:
                            var two = GetWormholeSymbol(sector, 2, "/");
                            var four = GetWormholeSymbol(sector,4,@"\");
                            line += string.Format(@"   {0}  {1}   ", two, four);
                            break;
                        case 2:
                            line += string.Format("|  {0},{1}", x, y).PadRight(9) + "|";
                            break;
                        case 3:
                            var twelve = GetWormholeSymbol(sector, 12, "|");
                            var six = GetWormholeSymbol(sector,6,"|");
                            var ownerLabel = sector.Owner != null ? sector.Owner.Color.ToString().Substring(0, 3) : "   ";
                            line += string.Format("{1}   {0}  {2}", ownerLabel, twelve,six);
                            break;
                        case 4:
                            line += "|        |";
                            break;
                        case 5:
                            var eight = GetWormholeSymbol(sector, 8, "/");
                            var ten = GetWormholeSymbol(sector,10,@"\");
                            line += string.Format(@"   {0}  {1}   ", ten, eight);
                            break;
                        default:
                            line += "          ";
                            break;
                    }
                }
            }
            _log.Log(line);
        }

        private static string GetWormholeSymbol(Sector sector, int facing, string noWormholdSymbol)
        {
            return sector.Wormholes.Contains(facing) ? "O" : noWormholdSymbol;
        }
    }
}
