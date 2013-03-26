using System.Collections.Generic;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Players
{
    public interface IPlayer
    {
        bool ChooseToClaimSector(Sector sector);
        bool ChooseToUseDiscovery(Discovery discoveryTile);
        IFactionType ChooseFaction(IEnumerable<IFactionType> availableFactions);
        MapLocation ChooseStartingLocation(IEnumerable<MapLocation> availableLocations);
        IAction ChooseAction(IEnumerable<IAction> validActions);
        void RotateSectorWormholes(Sector sector, int[] validFacings);
        MapLocation ChooseSectorLocation(IEnumerable<MapLocation> availableLocations);
        Tech ChooseResearch(IEnumerable<Tech> availableTech);
    }
}