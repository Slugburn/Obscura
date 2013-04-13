using System;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Builders
{
    public class Orbital : IBuildable
    {
        public void Place(Sector sector)
        {
            sector.HasOrbital = true;
            sector.Squares.Add(new PopulationSquare(ProductionType.Orbital, false) {Sector = sector});
        }

        public string Name
        {
            get { return "Orbital"; }
        }
    }
}