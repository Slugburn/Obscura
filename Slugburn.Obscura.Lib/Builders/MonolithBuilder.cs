using System.Linq;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Builders
{
    public class MonolithBuilder : BuilderBase
    {
        public MonolithBuilder() : base("Monolith")
        {
        }

        public override bool IsBuildAvailable(Faction faction)
        {
            return faction.Material >= CostFor(faction)
                   && faction.HasTechnology(Tech.Monolith)
                   && faction.Sectors.Any(IsValidPlacementLocation);
        }

        public override IBuildable Create(Faction faction)
        {
            return new Monolith();
        }

        public override bool IsValidPlacementLocation(Sector sector)
        {
            return !sector.HasMonolith;
        }

        public override int CostFor(Faction faction)
        {
            return faction.MonolithCost;
        }

        public override double CombatEfficiencyFor(Faction faction)
        {
            return 0;
        }

        public override bool OnePerSector
        {
            get { return true; }
        }
    }
}
