using System;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Builders
{
    public class Monolith : IBuildable
    {
        public override string ToString()
        {
            return "Monolith";
        }

        public void Place(Sector sector)
        {
            sector.HasMonolith = true;
            sector.Vp += 3;
        }

    }
}