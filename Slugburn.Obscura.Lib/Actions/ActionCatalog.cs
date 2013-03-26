using System.Collections.Generic;

namespace Slugburn.Obscura.Lib.Actions
{
    internal class ActionCatalog
    {
        static ActionCatalog()
        {
            All = new IAction[]
                      {
                          new ExploreAction(),
                          new ResearchAction(), 
                      };
        }

        public static IEnumerable<IAction> All { get; set; }
    }
}