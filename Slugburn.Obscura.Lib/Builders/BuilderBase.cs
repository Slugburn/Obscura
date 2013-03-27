using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Builders
{
    public abstract class BuilderBase : IBuilder
    {
        public abstract bool IsBuildAvailable(Faction faction);
        public abstract IBuildable Create(Faction faction);
        
        public virtual bool IsValidPlacementLocation(Sector sector)
        {
            return true;
        }
    }
}