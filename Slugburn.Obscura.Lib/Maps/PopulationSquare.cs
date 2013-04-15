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

        public Sector Sector { get; set; }
        public ProductionType ProductionType { get; set; }
        public bool Advanced { get; set; }

        public PlayerFaction Owner { get; set; }

        public override string ToString()
        {
            return Advanced ? "Advanced " + ProductionType : ProductionType.ToString();
        }
    }

}