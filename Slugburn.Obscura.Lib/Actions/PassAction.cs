using System.Linq;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Actions
{
    public class PassAction : IAction
    {
        public override string ToString()
        {
            return "Pass";
        }

        public void Do(PlayerFaction faction)
        {
            if (!faction.Game.Factions.Any(f => f.Passed))
                faction.Game.StartingFaction = faction;
            faction.Passed = true;
        }

        public bool IsValid(PlayerFaction faction)
        {
            return true;
        }
    }
}
