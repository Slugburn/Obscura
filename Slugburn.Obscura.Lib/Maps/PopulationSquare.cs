using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Maps
{
    public class PopulationSquare
    {
        public PopulationSquare(ProductionType productionType, bool advanced)
        {
            ProductionType = productionType;
            Advanced = advanced;
        }

        public ProductionType ProductionType { get; set; }
        public bool Advanced { get; set; }

        public Faction Owner { get; set; }
    }
}