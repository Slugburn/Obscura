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