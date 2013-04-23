using System;
using System.Linq;
using Slugburn.Obscura.Lib;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Test
{
    public class ConsoleGameView : IGameView
    {

        public void Display(Game game)
        {
            var map = game.Map;
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

        public void UpdateSector(Sector sector)
        {
        }

        private void WriteRow(SectorMap map, int y, int minY, int row, int minX, int maxX)
        {
            Console.Write("".PadRight((y - minY) * 5)); 
            for (var x = maxX; x >= minX; x--)
            {
                var sector = map.GetSector(new MapCoord(x,y));
                if (sector == null)
                {
                    Console.BackgroundColor = 0;
                    Console.Write("".PadRight(10));
                }
                else
                {
                    Console.ForegroundColor = sector.Owner == null ? 0 : sector.Owner.Color.ToConsoleColor();
                    switch (row)
                    {
                        case 1:
                            var two = GetWormholeSymbol(sector, 2, "/");
                            var four = GetWormholeSymbol(sector,4,@"\");
                            Console.Write(@"   {0}  {1}   ", two, four);
                            break;
                        case 2:
                            var ownerLabel = sector.Owner != null ? sector.Owner.Color.ToString().Substring(0, 3) : "   ";
                            Console.Write("|   {0}  |", ownerLabel);
                            break;
                        case 3:
                            var twelve = GetWormholeSymbol(sector, 12, "|");
                            var six = GetWormholeSymbol(sector,6,"|");
                            var coords = string.Format("{0},{1}", x, y).PadRight(4).PadLeft(5);
                            Console.Write("{1}  {0} {2}", coords, twelve,six);
                            break;
                        case 4:
                            Console.Write("|        |");
                            break;
                        case 5:
                            var eight = GetWormholeSymbol(sector, 8, "/");
                            var ten = GetWormholeSymbol(sector,10,@"\");
                            Console.Write(@"   {0}  {1}   ", ten, eight);
                            break;
                        default:
                            Console.Write("          ");
                            break;
                    }
                }
            }
            Console.WriteLine();
        }

        private static string GetWormholeSymbol(Sector sector, int facing, string noWormholdSymbol)
        {
            return sector.Wormholes.Contains(facing) ? "O" : noWormholdSymbol;
        }
    }
}
