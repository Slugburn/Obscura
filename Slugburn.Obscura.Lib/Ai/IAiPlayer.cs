using System.Collections.Generic;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Ai.Actions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Players;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Ai
{
    public interface IAiPlayer : IPlayer
    {
        Sector RallyPoint { get; set; }
        IEnumerable<IAction> ValidActions { get; set; }
        IList<BuildLocation> BuildList { get; set; }
        Tech TechToResearch { get; set; }
    }
}
