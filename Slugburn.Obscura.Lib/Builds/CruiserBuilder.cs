using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Builds
{
    public class CruiserBuilder : ShipBuilder
    {
        public CruiserBuilder() : base(faction=>faction.Cruiser, 4)
        {
        }
    }
}
