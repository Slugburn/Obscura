using System.Collections.Generic;
using Slugburn.Obscura.Lib.Combat;

namespace Slugburn.Obscura.Lib.Factions
{
    public interface IShipOwner
    {
        IEnumerable<Target> ChooseDamageDistribution(IEnumerable<DamageRoll> hits, IEnumerable<Target> targets);
    }
}
