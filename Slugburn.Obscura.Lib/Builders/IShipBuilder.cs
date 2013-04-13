using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Builders
{
    public interface IShipBuilder : IBuilder
    {
        int MaximumBuildableFor(Faction faction);
    }
}
