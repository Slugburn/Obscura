using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Builders
{
    public interface IBuildable
    {
        void Place(Sector sector);
        string Name { get; }
    }
}
