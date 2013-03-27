namespace Slugburn.Obscura.Lib.Builders
{
    public class InterceptorBuilder : ShipBuilder
    {
        public InterceptorBuilder() : base(faction=>faction.Interceptor, 8)
        {
        }
    }
}