using System.Linq;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Actions
{
    public class PassAction : IAction
    {
        public void Do(Faction faction)
        {
            if (!faction.Game.Factions.Any(f => f.Passed))
                faction.Game.StartingFaction = faction;
            faction.Passed = true;
        }

        public bool IsValid(Faction faction)
        {
            return true;
        }
    }
}
