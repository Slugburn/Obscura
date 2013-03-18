namespace Slugburn.Obscura.Lib.Actions
{
    public interface IAction
    {
        void Do(Player player);
        bool IsValid(Player player);
    }
}