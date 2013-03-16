namespace Slugburn.Obscura.Lib
{

    public class PopSpace
    {
        public ProductionType ProductionType { get; set; }
        public bool Advanced { get; set; }

        public PopSpace(ProductionType productionType, bool advanced)
        {
            ProductionType = productionType;
            Advanced = advanced;
        }
    }
}
