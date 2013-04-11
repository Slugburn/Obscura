using System.Collections.Generic;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Ai.Actions;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Players;

namespace Slugburn.Obscura.Lib.Ai
{
    public interface IAiPlayer : IPlayer
    {
        Sector RallyPoint { get; set; }
        IEnumerable<IAction> ValidActions { get; set; }
        List<BuildLocation> BuildList { get; set; }
        IList<IBuilder> GetBestBuildList();
    }
}
