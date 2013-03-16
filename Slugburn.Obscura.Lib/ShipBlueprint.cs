using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slugburn.Obscura.Lib
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
    }
}
