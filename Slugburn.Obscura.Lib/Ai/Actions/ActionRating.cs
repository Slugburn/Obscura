using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai.Actions
{
    public class ActionRating
    {
        public IAction Action { get; set; }
        public decimal Rating { get; set; }

        public ActionRating(IAction action, decimal rating)
        {
            Action = action;
            Rating = rating;
        }
    }
}