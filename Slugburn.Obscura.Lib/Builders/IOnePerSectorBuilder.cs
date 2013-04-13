using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Builders
{
    interface IOnePerSectorBuilder
    {
        bool HasBeenBuilt(Sector sector);
    }
}
