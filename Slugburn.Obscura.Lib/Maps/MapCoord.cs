using System;

namespace Slugburn.Obscura.Lib.Maps
{
    public class MapCoord
    {
        public MapCoord(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        protected bool Equals(MapCoord other)
        {
            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X*397) ^ Y;
            }
        }

        public MapCoord Go(int facing, int distance)
        {
            switch (facing)
            {
                case 12:
                    return new MapCoord(X + distance, Y);
                case 2:
                    return new MapCoord(X + distance, Y + distance);
                case 4:
                    return new MapCoord(X, Y + distance);
                case 6:
                    return new MapCoord(X - distance, Y);
                case 8:
                    return new MapCoord(X - distance, Y - distance);
                case 10:
                    return new MapCoord(X, Y - distance);
                default:
                    throw new ArgumentOutOfRangeException("facing");
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MapCoord) obj);
        }

        public override string ToString()
        {
            return string.Format("( {0} , {1} )", X, Y);
        }
    }
}