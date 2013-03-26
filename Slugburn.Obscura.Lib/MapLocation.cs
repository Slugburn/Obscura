using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Obscura.Lib
{
    public class MapLocation
    {
        public MapLocation(SectorMap map, MapCoord coord)
        {
            Map = map;
            Coord = coord;
        }

        protected MapLocation()
        {
        }

        public SectorMap Map { get; set; }
        public MapCoord Coord { get; set; }

        public virtual Sector Sector
        {
            get { return Map.GetSector(Coord); }
            set { Map.Place(value, Coord); }
        }

        public int DistanceFromCenter
        {
            get { return Math.Max(Math.Abs(Coord.X), Math.Abs(Coord.Y)); }
        }

        protected bool Equals(MapLocation other)
        {
            return Equals(Coord, other.Coord);
        }

        public override int GetHashCode()
        {
            return (Coord != null ? Coord.GetHashCode() : 0);
        }

        public IEnumerable<MapLocation> AdjacentExplorable()
        {
            return Sector.Wormholes
                .Select(facing => Coord.Go(facing, 1))
                .Where(coord => Map.GetSector(coord) == null)
                .Select(coord => new MapLocation(Map, coord));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MapLocation) obj);
        }

        public virtual IEnumerable<int> AdjacentWormholesFor(Player player)
        {
            return Facing.All.Select(facing => new {facing, sector = Map.GetSector(Coord.Go(facing, 1))})
                .Where(x => x.sector != null && x.sector.Owner == player && x.sector.Wormholes.Contains(Facing.Reverse(x.facing)))
                .Select(x=>x.facing);
        }
    }
}