using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Actions
{
    public interface IAction
    {
        void Do(Faction faction);
        bool IsValid(Faction faction);
        string Name { get; }
    }
}