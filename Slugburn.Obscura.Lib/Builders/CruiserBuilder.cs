namespace Slugburn.Obscura.Lib.Builders
{
    public class CruiserBuilder : ShipBuilder
    {
        public CruiserBuilder() : base(faction=>faction.Cruiser, 4)
        {
        }
    }
}
