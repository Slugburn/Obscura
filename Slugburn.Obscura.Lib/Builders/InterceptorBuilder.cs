namespace Slugburn.Obscura.Lib.Builders
{
    public class InterceptorBuilder : ShipBuilder
    {
        public InterceptorBuilder() : base("Interceptor", faction=>faction.Interceptor, 8)
        {
        }
    }
}