using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Obscura.Lib
{
    public class Sector
    {
        public Sector()
        {
            Planets = new List<Planet>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Vp { get; set; }

        public int Ancients { get; set; }

        public bool Discovery { get; set; }

        public List<Planet> Planets { get; private set; }

        public bool Artifact { get; set; }

        public int[] Wormholes { get; set; }

        internal void AddPlanet(PopSpace[] populationSpaces)
        {
            Planets.Add(new Planet(populationSpaces));
        }
    }
}
