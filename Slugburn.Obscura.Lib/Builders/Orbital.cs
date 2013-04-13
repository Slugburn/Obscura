using System;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Builders
{
    public class Orbital : IBuildable
    {
        public void Place(Sector sector)
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get { return "Orbital"; }
        }
    }
}