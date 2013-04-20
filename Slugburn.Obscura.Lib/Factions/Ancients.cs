using System.Collections.Generic;
using Slugburn.Obscura.Lib.Ai;
using Slugburn.Obscura.Lib.Combat;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Factions
{
    public class Ancients : IShipOwner
    {
        public override string ToString()
        {
            return "Ancients";
        }

        public IEnumerable<Target> ChooseDamageDistribution(IEnumerable<DamageRoll> hits, IEnumerable<Target> targets)
        {
            return AiPlayer.PickDamageDistribution(hits,targets);
        }

        public virtual void CreateShipsFor(Sector sector)
        {
            for (var i = 0; i < sector.Ancients; i++)
            {
                var ship = new AncientShip(this);
                sector.AddShip(ship);
            }
        }
    }
}
