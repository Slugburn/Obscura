using System;
using System.Linq;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Actions
{
    public class ResearchAction : IAction
    {
        public void Do(Faction faction)
        {
            var tech = faction.Player.ChooseResearch(faction.AvailableResearchTech());
            faction.Game.AvailableTechTiles.Remove(tech);
            faction.Technologies.Add(tech);
            faction.Science -= faction.CostFor(tech);
        }

        public bool IsValid(Faction faction)
        {
            return faction.AvailableResearchTech().Any();
        }
    }
}
