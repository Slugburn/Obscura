namespace Slugburn.Obscura.Lib.Builders
{
    public class DreadnoughtBuilder : ShipBuilder
    {
        public DreadnoughtBuilder() : base(faction=>faction.Dreadnought, 2)
        {
        }
    }
}
