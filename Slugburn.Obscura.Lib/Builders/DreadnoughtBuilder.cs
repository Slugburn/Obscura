namespace Slugburn.Obscura.Lib.Builders
{
    public class DreadnoughtBuilder : ShipBuilder
    {
        public DreadnoughtBuilder() : base("Dreadnought", faction=>faction.Dreadnought, 2)
        {
        }
    }
}
