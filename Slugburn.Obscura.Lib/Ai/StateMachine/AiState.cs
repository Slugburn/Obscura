using System.Collections.Generic;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Ai.StateMachine
{
    public class AiState
    {
        public AiState(Faction faction)
        {
            Faction = faction;
        }

        public Faction Faction { get; set; }

        public AiPlayer Player
        {
            get { return (AiPlayer) Faction.Player; }
        }

        public int Round
        {
            get { return Faction.Game.Round; }
        }

        public bool SavingForTech { get; set; }

        public IAction GetAction<T>() where T : IAction
        {
            return Player.GetAction<T>();
        }

        public static IAiDecision[] EconomicStrategy =
            new IAiDecision[]
                {
                    new Bankrupt(),
                    new EconomicResearch(),
                    new MilitaryResearch(),
                    new DefendSector(),
                    new Influence(),
                    new UnderThreat(), 
                    new ReinforceAttack(),
                    new BuildOrbital(),
                    new BuildMonolith(),
                    new Explore(),
                    new Attack(),
                    new ImproveFleet(), 
                };

        public static IEnumerable<IAiDecision> MilitaryStrategy =
            new IAiDecision[]
                {
                    new Bankrupt(), 
                    new DefendSector(), 
                    new Attack(),
                    new MilitaryResearch(),
                    new Influence(), 
                    new ReinforceAttack(),
                    new EconomicResearch(), 
                    new UnderThreat(),
                    new ImproveFleet(), 
                    new BuildOrbital(),
                    new BuildMonolith(),
                };

        public int RoundSpendingLimit
        {
            get { return Round == 10 ? 0 : Round + 5; }
        }

        public IAction ResearchOrSaveFor(Tech tech)
        {
            if (Faction.CostFor(tech) > Faction.Science)
            {
                SavingForTech = true;
                return null;
            }

            Player.TechToResearch = tech;
            return Faction.GetAction<ResearchAction>();
        }

        private void Log(string messageFormat, params object[] args)
        {
            Faction.Log(messageFormat, args);
        }
    }
}