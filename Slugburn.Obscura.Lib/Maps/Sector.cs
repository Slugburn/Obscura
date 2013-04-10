using System.Collections.Generic;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Maps
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

        public Faction Owner { get; set; }

        public void RotateClockwise()
        {
            for (var i = 0; i < Wormholes.Length; i++)
                Wormholes[i] = Facing.RotateClockwise(Wormholes[i], 2);
        }

        public void AddShip(Ship ship)
        {
            if (ship.Sector != null)
                ship.Sector._ships.Remove(ship);
            _ships.Add(ship);
            ship.Sector = this;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Name, Location);
        }
    }
}
