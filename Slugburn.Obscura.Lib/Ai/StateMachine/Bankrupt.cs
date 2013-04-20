using System;
using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.StateMachine
{
    class Bankrupt : IAiDecision
    {
        public IAction Decide(AiState state)
        {
            if (state.Player.SpendingInfluenceWillBankrupt() || state.Faction.Influence == 0)
                return state.Faction.GetAction<PassAction>();
            return null;
        }
    }
}
