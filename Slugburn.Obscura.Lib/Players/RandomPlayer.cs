using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Players
{
    public class RandomPlayer : IPlayer
    {
        public bool ChooseToClaimSector(Sector sector)
        {
            return true;
        }

        public bool ChooseToUseDiscovery(Discovery discoveryTile)
        {
            return false;
        }

        public IFactionType ChooseFaction(IEnumerable<IFactionType> availableFactions)
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

        public void RotateSectorWormholes(Sector sector, int[] validFacings)
        {
            while (!sector.Wormholes.Intersect(validFacings).Any())
                sector.RotateClockwise();
        }

        public MapLocation ChooseSectorLocation(IEnumerable<MapLocation> availableLocations)
        {
            return availableLocations.PickRandom();
        }

        public Tech ChooseResearch(IEnumerable<Tech> availableTech)
        {
            return availableTech.PickRandom();
        }
    }
}