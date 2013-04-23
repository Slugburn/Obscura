using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib
{
    public interface IGameView
    {
        void Display(Game game);
        void UpdateSector(Sector sector);
    }
}