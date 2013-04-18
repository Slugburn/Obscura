using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Ai.Actions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Ships;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Ai.Generators
{
    public class ResearchListGenerator
    {
        public decimal RateRallyPointForUpgrade(IAiPlayer player)
        {
            return Rate(player, player.RallyPoint.GetFriendlyShips(player.Faction).ToArray());
        }

        public decimal RateFleet(IAiPlayer player)
        {
            return Rate(player, player.Faction.Ships);
        }

        private decimal Rate(IAiPlayer player, IList<PlayerShip> ships)
        {
            var faction = player.Faction;
            var toUpgrade = ships;
            if (toUpgrade.Count == 0)
                return 0;

            if (player.GetAction<ResearchAction>() == null)
                return 0;

            var tech = GeneratePartTech(faction);
            player.TechToResearch = tech;
            if (tech == null)
                return 0;

            return toUpgrade.Count()*tech.Cost;
        }

        public Tech GeneratePartTech(PlayerFaction faction)
        {
            var bestPartTechCost = faction.Technologies
                .Where(tech => tech is PartTech)
                .Cast<PartTech>()
                .GroupBy(tech => tech.PartType)
                .ToDictionary(g => g.Key, g => g.Max(tech => tech.Cost));
            var improvedPartTechs = faction.Game.AvailableTechTiles
                .Where(tech => tech is PartTech && tech.Cost <= faction.Science)
                .Cast<PartTech>()
                .Select(tech => new {tech, rating = bestPartTechCost.ContainsKey(tech.PartType) ? tech.Cost - bestPartTechCost[tech.PartType] : tech.Cost})
                .Where(x => x.rating > 0)
                .ToArray();
            return EconomicResearchDecision.PickBestAvailableTech(faction, improvedPartTechs.Select(x => x.tech));
        }
    }
}
