using System;
using Slugburn.Obscura.Lib;

namespace Slugburn.Obscura.Test
{
    public class ConsoleLog : ILog
    {
        public void Log(string messageFormat, params object[] args)
        {
            Console.WriteLine(messageFormat, args);
        }
    }
}
