namespace Slugburn.Obscura.Lib.Builds
{
    public class DreadnoughtBuilder : ShipBuilder
    {
        public DreadnoughtBuilder() : base(faction=>faction.Dreadnought, 2)
        {
        }
    }
}
