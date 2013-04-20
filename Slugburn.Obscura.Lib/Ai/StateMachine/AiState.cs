using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Factions;

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

        public IAction GetAction<T>() where T : IAction
        {
            return Player.GetAction<T>();
        }

        public static IAiDecision[] Decisions =
            new IAiDecision[]
                {
                    new Bankrupt(),
                    new EconomicResearch(),
                    new MilitaryResearch(),
                    new UnderAttack(),
                    new Influence(),
                    new UnderThreat(), 
                    new ReinforceAttack(),
                    new BuildOrbital(),
                    new BuildMonolith(),
                    new Explore(),
                    new Attack(),
                    new ImproveFleet(), 
                };
    }
}