using System;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Test
{
    public static class TestExtensions
    {
        public static ConsoleColor ToConsoleColor(this FactionColor color)
        {
            switch (color)
            {
                case FactionColor.Yellow:
                    return ConsoleColor.Yellow;
                case FactionColor.Red:
                    return ConsoleColor.Red;
                case FactionColor.Green:
                    return ConsoleColor.Green;
                case FactionColor.Blue:
                    return ConsoleColor.Blue;
                case FactionColor.Black:
                    return ConsoleColor.Black;
                case FactionColor.Undefined:
                    return 0;
                case FactionColor.White:
                    return ConsoleColor.White;
                default:
                    return 0;
            }
        }
    }
}
