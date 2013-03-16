namespace Slugburn.Obscura.Lib
{
    public class Planet
    {
        public PopSpace[] PopulationSpaces { get; private set; }

        public Planet(PopSpace[] populationSpaces)
        {
            PopulationSpaces = populationSpaces;
        }
    }
}