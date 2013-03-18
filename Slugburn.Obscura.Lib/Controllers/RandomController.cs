using System.Collections.Generic;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Controllers
{
    public class RandomController : IPlayerController
    {
        public bool ChooseToClaimSector(Sector sector)
        {
            return true;
        }

        public bool ChooseToUseDiscovery(Discovery discoveryTile)
        {
            return false;
        }

        public IFaction ChooseFaction(IEnumerable<IFaction> availableFactions)
        {
            return availableFactions.PickRandom();
        }

        public MapLocation ChooseStartingLocation(IEnumerable<MapLocation> availableLocations)
        {
            return availableLocations.PickRandom();
        }

        public IAction ChooseAction(IEnumerable<IAction> validActions)
        {
            return validActions.PickRandom();
        }
    }
}