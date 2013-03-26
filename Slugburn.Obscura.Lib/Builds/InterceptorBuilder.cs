namespace Slugburn.Obscura.Lib.Builds
{
    public class InterceptorBuilder : ShipBuilder
    {
        public InterceptorBuilder() : base(faction=>faction.Interceptor, 8)
        {
        }
    }
}