using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Players;

namespace Slugburn.Obscura.Lib.Ai
{
    public interface IAiPlayer : IPlayer
    {
        Sector RallyPoint { get; set; }
    }
}
