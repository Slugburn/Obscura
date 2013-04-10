using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Players;

namespace Slugburn.Obscura.Lib.Ai
{
    public interface IDecision<TResult>
    {
        DecisionResult<TResult> Decide(IAiPlayer player);
    }
}
