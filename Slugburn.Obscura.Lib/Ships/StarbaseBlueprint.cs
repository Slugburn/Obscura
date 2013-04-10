using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Obscura.Lib.Ships
{
    public class StarbaseBlueprint : ShipBlueprint
    {
        public override bool IsProfileValid(ShipProfile profile)
        {
            // Needs to have zero drive and non-negative energy
            return profile.Move == 0 && profile.Energy >= 0;
        }
    }
}
