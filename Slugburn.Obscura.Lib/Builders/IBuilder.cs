using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Builders
{
    public interface IBuilder
    {
        bool IsBuildAvailable(Faction faction);
        IBuildable Create(Faction faction);
        bool IsValidPlacementLocation(Sector sector);
        int CostFor(Faction faction);
    }
}