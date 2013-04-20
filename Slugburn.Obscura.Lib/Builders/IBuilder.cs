using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Builders
{
    public interface IBuilder
    {
        string Name { get; }
        bool IsBuildAvailable(Faction faction);
        IBuildable Create(Faction faction);
        bool IsValidPlacementLocation(Sector sector);
        int CostFor(Faction faction);
        decimal CombatRatingFor(Faction faction);
        decimal CombatEfficiencyFor(Faction faction);
        bool OnePerSector { get; }
        Tech RequiredTech { get; }
    }
}