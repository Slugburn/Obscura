using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slugburn.Obscura.Lib.Actions;

namespace Slugburn.Obscura.Lib.Ai
{
    public static class AiExtensions
    {
        public static IAction GetAction<T>(this IAiPlayer player) where T:IAction
        {
            return player.ValidActions.SingleOrDefault(a => a is T);
        }
    }
}
