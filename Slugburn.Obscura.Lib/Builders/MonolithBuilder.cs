using System.Linq;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Builders
{
    public class MonolithBuilder : BuilderBase, IOnePerSectorBuilder
    {
        public MonolithBuilder() : base("Monolith")
        {
        }

        public override IBuildable Create(PlayerFaction faction)
        {
            return new Monolith();
        }

        public override Tech RequiredTech
        {
            get { return Tech.Monolith; }
        }

        public override bool IsValidPlacementLocation(Sector sector)
        {
            return !sector.HasMonolith;
        }

        public override int CostFor(PlayerFaction faction)
        {
            return faction.MonolithCost;
        }

        public override decimal CombatRatingFor(PlayerFaction faction)
        {
            return 0;
        }

        public bool HasBeenBuilt(Sector sector)
        {
            return sector.HasMonolith;
        }
    }
}
