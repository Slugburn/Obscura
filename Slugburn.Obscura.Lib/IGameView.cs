using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib
{
    public interface IGameView
    {
        void Display(Game game);
        void UpdateSector(Sector sector);
        void MoveShip(Ship ship, Sector start, Sector end);
    }
}