using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class ImproveFleetDecision : IActionDecision
    {
        private readonly BuildListGenerator _buildListGenerator;
        private readonly UpgradeListGenerator _upgradeListGenerator;
        private readonly ILog _log;

        public ImproveFleetDecision(BuildListGenerator buildListGenerator, UpgradeListGenerator upgradeListGenerator, ILog log)
        {
            _buildListGenerator = buildListGenerator;
            _upgradeListGenerator = upgradeListGenerator;
            _log = log;
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
                
                // don't build unless we can fully utilize the build action
                if (player.BuildList != null && player.BuildList.Count == faction.BuildCount)
                {
                    var rating = player.BuildList.Select(x => x.Builder).GetTotalCombatRatingFor(faction);
                    possibleActions.Add(new ActionRating(build, rating));
                }
            }

            if (upgrade!=null)
            {
                player.UpgradeList = _upgradeListGenerator.Generate(player);
                var rating = player.UpgradeList != null ? player.UpgradeList.Sum(x => x.RatingImprovement) : 0;
                if (rating > 10)
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
                    .Select(tech => new { tech, rating = bestPartTechCost.ContainsKey(tech.PartType) ? tech.Cost -  bestPartTechCost[tech.PartType] : tech.Cost })
                    .Where(x=>x.rating > 0)
                    .ToArray();
                var toResearch = EconomicResearchDecision.PickBestAvailableTech(faction, improvedPartTechs.Select(x=>x.tech));
                if (toResearch!=null)
                {
                    player.TechToResearch = toResearch;
                    possibleActions.Add(new ActionRating(research, improvedPartTechs.First(x => toResearch == x.tech).rating * faction.Ships.Count));
                }
            }

            if (possibleActions.Count == 0)
                return new ActionDecisionResult(new ExploreDecision());

            // return action with highest rating
            possibleActions.Each(x => _log.Log("\t\t{0} [{1:n2}]", x.Action, x.Rating));
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