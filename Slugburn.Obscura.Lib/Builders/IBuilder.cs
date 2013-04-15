using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Builders
{
    public interface IBuilder
    {
        string Name { get; }
        bool IsBuildAvailable(PlayerFaction faction);
        IBuildable Create(PlayerFaction faction);
        bool IsValidPlacementLocation(Sector sector);
        int CostFor(PlayerFaction faction);
        decimal CombatRatingFor(PlayerFaction faction);
        decimal CombatEfficiencyFor(PlayerFaction faction);
        bool OnePerSector { get; }
        Tech RequiredTech { get; }
    }
}