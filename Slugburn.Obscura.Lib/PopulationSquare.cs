namespace Slugburn.Obscura.Lib
{

    public class PopulationSquare
    {
        public ProductionType ProductionType { get; set; }
        public bool Advanced { get; set; }

        public PopulationSquare(ProductionType productionType, bool advanced)
        {
            ProductionType = productionType;
            Advanced = advanced;
        }
    }
}
