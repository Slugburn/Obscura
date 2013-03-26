using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Builds
{
    public interface IBuilder
    {
        bool IsValid(Faction faction);
        IBuildable Create(Faction faction);
    }
}