using System;
using System.Linq;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Builds
{
    public abstract class ShipBuilder : BuilderBase
    {
        private readonly Func<Faction, ShipBlueprint> _blueprintAccessor;
        private readonly int _maxCount;

        protected ShipBuilder(Func<Faction,ShipBlueprint> blueprintAccessor, int maxCount)
        {
            _blueprintAccessor = blueprintAccessor;
            _maxCount = maxCount;
        }

        public override bool IsValid(Faction faction)
        {
            var blueprint = _blueprintAccessor(faction);
            return faction.Materials > blueprint.Cost && faction.Ships.Count(ship => ship.Blueprint == blueprint) < _maxCount;
        }

        public override IBuildable Create(Faction faction)
        {
            var blueprint = _blueprintAccessor(faction);
            faction.Materials -= blueprint.Cost;
            return faction.CreateShip(blueprint);
        }
    }
}