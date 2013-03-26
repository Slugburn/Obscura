using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Builds
{
    public abstract class BuilderBase : IBuilder
    {
        public abstract bool IsValid(Faction faction);
        public abstract IBuildable Create(Faction faction);
    }
}