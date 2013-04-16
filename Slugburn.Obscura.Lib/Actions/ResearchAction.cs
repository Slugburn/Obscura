using System;
using System.Linq;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Actions
{
    public class ResearchAction : IAction
    {
        private readonly ILog _log;

        public ResearchAction(ILog log)
        {
            _log = log;
        }

        public override string ToString()
        {
            return "Research";
        }

        public void Do(PlayerFaction faction)
        {
            var tech = faction.Player.ChooseResearch(faction.AvailableResearchTech());
            faction.Game.AvailableTechTiles.Remove(tech);
            faction.Technologies.Add(tech);
            var cost = faction.CostFor(tech);
            faction.Science -= cost;
            _log.Log("\t{0} researched ({1} Science)", tech, cost);
            var effectTech = tech as EffectTech;
            if (effectTech != null)
                effectTech.OnDiscovery(faction);
        }

        public bool IsValid(PlayerFaction faction)
        {
            return !faction.Passed && faction.Influence > 0 && faction.AvailableResearchTech().Any();
        }
    }
}
