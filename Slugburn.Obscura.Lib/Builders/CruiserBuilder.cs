namespace Slugburn.Obscura.Lib.Builders
{
    public class CruiserBuilder : ShipBuilder
    {
        public CruiserBuilder() : base("Cruiser", faction=>faction.Cruiser, 4)
        {
        }
    }
}
