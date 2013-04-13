using System.Linq;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Builders
{
    public class OrbitalBuilder : BuilderBase
    {
        public OrbitalBuilder() : base("Orbital")
        {
        }

        public override bool IsBuildAvailable(Faction faction)
        {
            return faction.Material >= faction.OrbitalCost
                   && faction.HasTechnology(Tech.Orbital)
                   && faction.Sectors.Any(IsValidPlacementLocation);
        }

        public override IBuildable Create(Faction faction)
        {
            return new Orbital();
        }

        public override int CostFor(Faction faction)
        {
            return faction.OrbitalCost;
        }

        public override double CombatEfficiencyFor(Faction faction)
        {
            return 0;
        }

        public override bool IsValidPlacementLocation(Sector sector)
        {
            return !sector.HasOrbital;
        }
    }
}
