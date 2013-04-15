using System;

namespace Slugburn.Obscura.Lib.Ai
{
    public class DecisionResult<TResult>
    {
        public DecisionResult(IDecision<TResult> nextDecision)
        {
            NextDecision = nextDecision;
        }

        public DecisionResult(TResult result)
        {
            Result = result;
        }

        public TResult Result { get; set; }
        public IDecision<TResult> NextDecision { get; set; }
    }
}
