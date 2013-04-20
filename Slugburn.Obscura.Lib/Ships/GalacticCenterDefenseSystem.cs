using System.Collections.Generic;
using Slugburn.Obscura.Lib.Ai;
using Slugburn.Obscura.Lib.Combat;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Ships
{
    public class GalacticCenterDefenseSystem : Ship
    {
        public GalacticCenterDefenseSystem()
        {
            Faction = new Owner();
        }

        private static readonly ShipProfile _profile = new ShipProfile
            {
                Accuracy = 1,
                Cannons = new[] {1, 1, 1, 1},
                Structure = 7
            };
        public override ShipProfile Profile
        {
            get { return _profile; }
        }

        public override ShipType ShipType
        {
            get { return ShipType.GCDS; }
        }

        private class Owner : IShipOwner
        {
            public override string ToString()
            {
                return "Galactic Core Defense";
            }

            public IEnumerable<Target> ChooseDamageDistribution(IEnumerable<DamageRoll> hits, IEnumerable<Target> targets)
            {
                return AiPlayer.PickDamageDistribution(hits, targets);
            }

        }

    }
}
