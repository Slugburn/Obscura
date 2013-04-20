using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.StateMachine
{
    public interface IAiDecision
    {
        IAction Decide(AiState state);
    }
}
