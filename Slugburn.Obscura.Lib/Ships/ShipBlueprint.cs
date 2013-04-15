using System;
using System.Collections.Generic;

namespace Slugburn.Obscura.Lib.Ships
{
    public class ShipBlueprint
    {
        public ShipBlueprint()
        {
            Parts = new List<ShipPart>();
        }

        public int Cost { get; set; }

        public int PartSpaces { get; set; }

        public IList<ShipPart> Parts { get; set; }

        public int BaseInitiative { get; set; }

        public int BaseEnergy { get; set; }

        public string Name { get; set; }

        public ShipProfile Profile
        {
            get { return ShipProfile.Create(this, Parts); }
        }

        public decimal Rating
        {
            get { return Profile.Rating; }
        }

        public ShipType ShipType { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public bool IsPartListValid(IList<ShipPart> parts)
        {
            if (parts.Count > PartSpaces) 
                return false;
            var profile = ShipProfile.Create(this, parts);
            return IsProfileValid(profile);
        }

        public virtual bool IsProfileValid(ShipProfile profile)
        {
            // Needs to have positive drive and non-negative energy
            return profile.Move > 0 && profile.Energy >= 0;
        }

        public void Upgrade(ShipPart upgrade, ShipPart replace)
        {
            if (replace != null)
                Parts.Remove(replace);
            Parts.Add(upgrade);
            if (!IsPartListValid(Parts))
                throw new Exception(string.Format("Ship blueprint part list is not valid: {0} [{1}]", string.Join(",", Parts), PartSpaces));
        }
    }
}
