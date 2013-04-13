using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class DefendRallyPointDecision : IActionDecision
    {
        private readonly BuildListGenerator _generator;

        public DefendRallyPointDecision(BuildListGenerator generator)
        {
            _generator = generator;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;

            // Need to decide whether we should build, upgrade, or move
            var build = player.GetAction<BuildAction>();
            var upgrade = player.GetAction<UpgradeAction>();
            var move = player.GetAction<MoveAction>();
            var explore = player.GetAction<ExploreAction>();

            if (build!=null)
            {
                var buildList = _generator.Generate(faction, new[] {player.RallyPoint}, builder=>builder.CombatEfficiencyFor(faction));
                player.BuildList = buildList;
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