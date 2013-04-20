using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.StateMachine
{
    class Explore : IAiDecision
    {
        public IAction Decide(AiState state)
        {
            return state.GetAction<ExploreAction>();
        }
    }
}
