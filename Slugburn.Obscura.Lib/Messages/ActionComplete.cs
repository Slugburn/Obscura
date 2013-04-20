using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Messages
{
    public class ActionComplete
    {
        public ActionComplete(IAction action)
        {
            Action = action;
        }

        public IAction Action { get; set; }
    }
}