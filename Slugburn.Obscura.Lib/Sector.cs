using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib
{
    public class Sector
    {
        public Sector()
        {
            Ships = new List<Ship>();
        }

        public List<Ship> Ships { get; set; }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Vp { get; set; }

        public int Ancients { get; set; }

        public bool HasDiscovery { get; set; }

        public bool HasArtifact { get; set; }

        public int[] Wormholes { get; set; }

        public Discovery DiscoveryTile { get; set; }

        public PopulationSquare[] Population { get; set; }
        
        public bool IsInner
        {
            get { return Id > 100 && Id < 200; }
        }

        public bool IsMiddle
        {
            get { return Id > 200 && Id < 221; }
        }

        public bool IsOuter
        {
            get { return Id > 300; }
        }

    }
}
