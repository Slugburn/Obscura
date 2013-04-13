using System.Linq;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Builders
{
    public class OrbitalBuilder : BuilderBase, IOnePerSectorBuilder
    {
        public OrbitalBuilder() : base("Orbital")
        {
        }

        public override IBuildable Create(Faction faction)
        {
            return new Orbital();
        }

        public override int CostFor(Faction faction)
        {
            return faction.OrbitalCost;
        }

        public override decimal CombatEfficiencyFor(Faction faction)
        {
            return 0;
        }

        public override Tech RequiredTech
        {
            get { return Tech.Orbital; }
        }

        public override bool IsValidPlacementLocation(Sector sector)
        {
            return !sector.HasOrbital;
        }

        public bool HasBeenBuilt(Sector sector)
        {
            return sector.HasOrbital;
        }
    }
}
