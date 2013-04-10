namespace Slugburn.Obscura.Lib.Ai
{
    public static class DecisionExtensions
    {
        public static TResult GetResult<TResult>(this IDecision<TResult> rootDecision, IAiPlayer player)
        {
            var decision = rootDecision;
            while(true)
            {
                var result = decision.Decide(player);
                decision = result.NextDecision;
                if (decision == null)
                    return result.Result;
            } 
        }
    }
}
