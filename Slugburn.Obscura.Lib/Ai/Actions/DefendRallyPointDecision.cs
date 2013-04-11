using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class DefendRallyPointDecision : IActionDecision
    {
        public DefendRallyPointDecision()
        {
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            // Need to decide whether we should build, upgrade, or move
            var build = player.GetAction<BuildAction>();
            var upgrade = player.GetAction<UpgradeAction>();
            var move = player.GetAction<MoveAction>();
            var explore = player.GetAction<ExploreAction>();

            if (build!=null)
            {
                var buildList = player.GetBestBuildList();
                player.BuildList = buildList.Select(x => new BuildLocation { Builder = x, Location = player.RallyPoint }).ToList();
                return new ActionDecisionResult(build);
            }
            else if (explore!=null)
            {
                return new ActionDecisionResult(explore);
            }
            {
                return new ActionDecisionResult(player.GetAction<PassAction>());
            }
        }
    }

    public class BuildLocation
    {
        public IBuilder Builder { get; set; }

        public Sector Location { get; set; }
    }
}