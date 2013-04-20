using System;
using Slugburn.Obscura.Lib;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Test
{
    public class ConsoleLog : ILog
    {
        public void Log(string messageFormat, params object[] args)
        {
            Console.WriteLine(messageFormat, args);
        }

        public void Log(FactionColor color, string messageFormat, params object[] args)
        {
            Console.BackgroundColor = color.ToConsoleColor();
            Console.WriteLine(messageFormat, args);
        }
    }
}
