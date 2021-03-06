using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Slugburn.Obscura.Lib.Ships
{
    public class ShipProfile
    {
        public ShipProfile()
        {
            Cannons=new int[0];
            Missiles =new int[0];
        }

        public int[] Cannons { get; set; }

        public int[] Missiles { get; set; }

        public int Initiative { get; set; }

        public int Energy { get; set; }

        public int Accuracy { get; set; }

        public int Structure { get; set; }

        public int Deflection { get; set; }

        public int Move { get; set; }

        public static ShipProfile Create(ShipBlueprint blueprint, IList<ShipPart> parts)
        {
            return new ShipProfile
                       {
                           Cannons = parts.Where(x => x.Cannons != null).SelectMany(x => x.Cannons).ToArray(),
                           Missiles = parts.Where(x => x.Missiles != null).SelectMany(x => x.Missiles).ToArray(),
                           Initiative = blueprint.BaseInitiative + parts.Sum(x => x.Initiative),
                           Energy = blueprint.BaseEnergy + parts.Sum(x => x.Energy),
                           Accuracy = parts.Sum(x => x.Accuracy),
                           Structure = parts.Sum(x => x.Structure),
                           Deflection = parts.Sum(x => x.Deflection),
                           Move = parts.Sum(x => x.Move)
                       };
        }

        public decimal Rating
        {
            get
            {
                var cannonRating = Cannons.Sum();
                var missileRating = Missiles != null ? Missiles.Sum() : 0m;
                var offenseMultiplier = (1m + Accuracy * (1m+ Initiative/10m) + Initiative/10m);
                var defenseMultipler = (1m + Structure)* (1m - Deflection);
                return (cannonRating*offenseMultiplier*defenseMultipler + missileRating*offenseMultiplier) * (1m+Move/3m) * (1m+Energy/10m);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("I" + Initiative);
            sb.Append("M" + Move);
            sb.Append("S" + Structure);
            if (Accuracy > 0)
                sb.Append("A+" + Accuracy);
            if (Deflection < 0)
                sb.Append("D" + Deflection);
            if (Cannons.Any())
                sb.Append("[" + string.Join("", Cannons) + "]");
            if (Missiles.Any())
                sb.Append("<" + string.Join("", Missiles) + ">");
            return sb.ToString();
        }
    }
}