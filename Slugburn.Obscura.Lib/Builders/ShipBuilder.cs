using System;
using System.Linq;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Ships;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Builders
{
    public abstract class ShipBuilder : BuilderBase, IShipBuilder
    {
        private readonly Func<PlayerFaction, ShipBlueprint> _blueprintAccessor;
        private readonly int _maxCount;

        protected ShipBuilder(string name, Func<PlayerFaction,ShipBlueprint> blueprintAccessor, int maxCount) :base(name)
        {
            _blueprintAccessor = blueprintAccessor;
            _maxCount = maxCount;
        }

        public int MaximumBuildableFor(PlayerFaction faction)
        {
            var blueprint = _blueprintAccessor(faction);
            return _maxCount - faction.Ships.Count(ship => ship.Blueprint == blueprint);
        }

        public virtual bool CanMove
        {
            get { return true; }
        }

        public override IBuildable Create(PlayerFaction faction)
        {
            var cost = CostFor(faction);
            faction.Material -= cost;
            return faction.CreateShip(_blueprintAccessor(faction));
        }

        public override int CostFor(PlayerFaction faction)
        {
            return _blueprintAccessor(faction).Cost;
        }

        public override decimal CombatRatingFor(PlayerFaction faction)
        {
            return _blueprintAccessor(faction).Rating;
        }

        public override Tech RequiredTech
        {
            get { return null; }
        }

    }
}