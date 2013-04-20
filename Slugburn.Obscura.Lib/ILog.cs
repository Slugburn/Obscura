using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib
{
    public interface ILog
    {
        void Log(string messageFormat, params object[] args);
        void Log(FactionColor color, string messageFormat, params object[] args);
    }
}