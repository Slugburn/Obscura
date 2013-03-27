using System;
using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Obscura.Lib.Ships
{
    public class ShipBlueprint
    {
        public ShipBlueprint()
        {
            Parts = new List<ShipPart>();
            IsPartListValid = parts => DefaultIsPartListValid(this, parts);
        }

        public int Cost { get; set; }

        public int PartSpaces { get; set; }

        public IList<ShipPart> Parts { get; set; }

        public int BaseInitiative { get; set; }

        public int BasePower { get; set; }

        public int Initiative
        {
            get { return BaseInitiative + Parts.Sum(x => x.Initiative); }
        }

        public int Power
        {
            get { return BasePower + Parts.Sum(x => x.Power); }
        }

        public int Accuracy
        {
            get { return Parts.Sum(x => x.Accuracy); }
        }

        public int Structure
        {
            get { return Parts.Sum(x => x.Structure); }
        }

        public int Deflection
        {
            get { return Parts.Sum(x => x.Deflection); }
        }

        public int Move
        {
            get { return Parts.Sum(x => x.Move); }
        }

        public Func<IList<ShipPart>, bool> IsPartListValid { get; set;}

        private static bool DefaultIsPartListValid(ShipBlueprint blueprint, IList<ShipPart> parts)
        {
            if (parts.Count > blueprint.PartSpaces) 
                return false;

            // Needs to have positive drive and non-negative power
            var move = parts.Sum(x => x.Move);
            var power = parts.Sum(x => x.Power);
            return move > 0 && power >= 0;
        }

        public void Upgrade(ShipPart upgrade, ShipPart replace)
        {
            if (replace != null)
                Parts.Remove(replace);
            Parts.Add(upgrade);
            if (!IsPartListValid(Parts))
                throw new Exception("Ship blueprint part list is not valid.");
        }
    }
}
