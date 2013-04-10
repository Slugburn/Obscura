namespace Slugburn.Obscura.Lib.Ai
{
    public interface IDecision<TResult>
    {
        DecisionResult<TResult> Decide(IAiPlayer player);
    }
}
