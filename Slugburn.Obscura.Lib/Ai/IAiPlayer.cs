using System.Collections.Generic;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Players;
using Slugburn.Obscura.Lib.Ships;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Ai
{
    public interface IAiPlayer : IPlayer
    {
        Sector RallyPoint { get; set; }
        Sector ThreatPoint { get; set; }
        Sector StagingPoint { get; set; }
        IEnumerable<IAction> ValidActions { get; set; }
        IList<ShipMove> MoveList { get; set; }
        IList<BuildLocation> BuildList { get; set; }
        Tech TechToResearch { get; set; }
        IList<BlueprintUpgrade> UpgradeList { get; set; }
        IList<InfluenceLocation> InfluenceList { get; set; }
        IList<ShipPart> GetIdealPartList(ShipBlueprint blueprint);
        int ActionRatingMinimum { get; }
    }
}
