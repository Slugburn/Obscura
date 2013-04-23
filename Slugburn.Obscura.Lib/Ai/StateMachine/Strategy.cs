using System.Collections.Generic;

namespace Slugburn.Obscura.Lib.Ai.StateMachine
{
    public class Strategy
    {
        public static IAiDecision[] Economic =
            new IAiDecision[]
                {
                    new Bankrupt(),
                    new EconomicResearch(),
                    new Influence(),
                    new MilitaryResearch(),
                    new DefendSector(),
                    new UnderThreat(), 
                    new ReinforceAttack(),
                    new BuildOrbital(),
                    new BuildMonolith(),
                    new Explore(),
                    new Attack(),
                    new ImproveFleet(), 
                };

        public static IEnumerable<IAiDecision> Military =
            new IAiDecision[]
                {
                    new Bankrupt(), 
                    new MilitaryResearch(),
                    new DefendSector(), 
                    new Influence(), 
                    new Attack(),
                    new ReinforceAttack(),
                    new EconomicResearch(), 
                    new UnderThreat(),
                    new ImproveFleet(), 
                    new BuildOrbital(),
                    new BuildMonolith(),
                };
    }
}
