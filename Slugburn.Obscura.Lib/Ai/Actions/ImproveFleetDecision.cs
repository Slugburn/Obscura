using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class ImproveFleetDecision : IActionDecision
    {
        private readonly BuildListGenerator _buildListGenerator;
        private readonly UpgradeListGenerator _upgradeListGenerator;

        public ImproveFleetDecision(BuildListGenerator buildListGenerator, UpgradeListGenerator upgradeListGenerator)
        {
            _buildListGenerator = buildListGenerator;
            _upgradeListGenerator = upgradeListGenerator;
        }

        public DecisionResult<IAction> Decide(IAiPlayer player)
        {
            var faction = player.Faction;
            var possibleActions = new List<ActionRating>();

            // possible actions are building ships, upgrading ships, researching new technology
            var build = player.GetAction<BuildAction>();
            var upgrade = player.GetAction<UpgradeAction>();
            var research = player.GetAction<ResearchAction>();

            if (build!= null)
            {
                var closestSectorToRallyPoint = faction.GetClosestSectorTo(player.RallyPoint);
                player.BuildList = _buildListGenerator.Generate(faction, new[] { closestSectorToRallyPoint }, BuildListGenerator.RateAttackEfficency);
                var rating = player.BuildList.Select(x => x.Builder).GetTotalCombatRatingFor(faction);
                possibleActions.Add(new ActionRating(build,rating));
            }
            if (upgrade!=null)
            {
                player.UpgradeList = _upgradeListGenerator.Generate(player);
                var rating = player.UpgradeList.Sum(x => x.RatingImprovement);
                if (rating > 0)
                    possibleActions.Add(new ActionRating(upgrade,rating));
            }

            if (research!=null)
            {
                var bestPartTechCost = faction.Technologies
                    .Where(tech => tech is PartTech)
                    .Cast<PartTech>()
                    .GroupBy(tech=>tech.PartType)
                    .ToDictionary(g=>g.Key, g=>g.Max(tech=>tech.Cost));
                var improvedPartTechs = faction.Game.AvailableTechTiles
                    .Where(tech => tech is PartTech && tech.Cost <= faction.Science)
                    .Cast<PartTech>()
                    .Where(tech=>!bestPartTechCost.ContainsKey(tech.PartType) || bestPartTechCost[tech.PartType] < tech.Cost)
                    .ToArray();
                var toResearch = ResearchDecision.PickBestAvailableTech(faction, improvedPartTechs);
                if (toResearch!=null)
                {
                    player.TechToResearch = toResearch;
                    possibleActions.Add(new ActionRating(research, toResearch.Cost * faction.Ships.Count));
                }
            }

            if (possibleActions.Count == 0)
                return new ActionDecisionResult(new ExploreDecision());

            // return action with highest rating
            var action = possibleActions.OrderByDescending(x => x.Rating).First().Action;
            return new ActionDecisionResult(action);
        }

    }

    public class ActionRating
    {
        public IAction Action { get; set; }
        public decimal Rating { get; set; }

        public ActionRating(IAction action, decimal rating)
        {
            Action = action;
            Rating = rating;
        }
    }

}