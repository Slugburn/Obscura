using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class ActionDecisionResult : DecisionResult<IAction>
    {
        public ActionDecisionResult(IDecision<IAction> nextDecision) : base(nextDecision)
        {
        }

        public ActionDecisionResult(IAction result) : base(result)
        {
        }
    }
}
