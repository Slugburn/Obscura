using System.Collections.Generic;

namespace Slugburn.Obscura.Lib.Ships
{
    public class ShipPart
    {
        protected bool Equals(ShipPart other)
        {
            return string.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public string Name { get; set; }

        public int[] Cannons { get; set; }

        public int[] Missiles { get; set; }

        public int Initiative { get; set; }

        public int Energy { get; set; }

        public int Accuracy { get; set; }

        public int Structure { get; set; }

        public int Deflection { get; set; }

        public int Move { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ShipPart) obj);
        }
    }
}
