namespace Slugburn.Obscura.Lib
{
    public interface ILog
    {
        void Log(string messageFormat, params object[] args);
    }
}