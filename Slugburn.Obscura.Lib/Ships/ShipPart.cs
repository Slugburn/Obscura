namespace Slugburn.Obscura.Lib.Ships
{
    public class ShipPart
    {
        public string Name { get; set; }

        public int Initiative { get; set; }

        public int Power { get; set; }

        public int Accuracy { get; set; }

        public int Structure { get; set; }

        public int Deflection { get; set; }

        public int[] Damage { get; set; }

        public int Move { get; set; }

        public bool FirstStrike { get; set; }
    }
}
