namespace Slugburn.Obscura.Lib
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

        public Player Owner { get; set; }
    }
}