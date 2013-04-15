using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Actions
{
    public interface IAction
    {
        void Do(PlayerFaction faction);
        bool IsValid(PlayerFaction faction);
    }
}