using System.Collections.Generic;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Controllers
{
    public interface IPlayerController
    {
        bool ChooseToClaimSector(Sector sector);
        bool ChooseToUseDiscovery(Discovery discoveryTile);
        IFaction ChooseFaction(IEnumerable<IFaction> availableFactions);
        MapLocation ChooseStartingLocation(IEnumerable<MapLocation> availableLocations);
        IAction ChooseAction(IEnumerable<IAction> validActions);
    }
}