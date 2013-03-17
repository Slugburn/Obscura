using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib
{
    public class Sector
    {
        private readonly List<Ship> _ships;

        public Sector()
        {
            _ships = new List<Ship>();
        }

        public IEnumerable<Ship> Ships
        {
            get { return _ships; }
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Vp { get; set; }

        public int Ancients { get; set; }

        public bool HasDiscovery { get; set; }

        public bool HasArtifact { get; set; }

        public int[] Wormholes { get; set; }

        public Discovery DiscoveryTile { get; set; }

        public PopulationSquare[] Squares { get; set; }
        
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

        public MapLocation Location { get; set; }

        public Player Owner { get; set; }

        public void AddShip(Ship ship)
        {
            _ships.Add(ship);
        }
    }
}
