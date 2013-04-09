using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;
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

        public IBuilder ChooseBuilder(IEnumerable<IBuilder> validBuilders)
        {
            return validBuilders.PickRandom();
        }

        public Sector ChoosePlacementLocation(IBuildable built, List<Sector> validPlacementLocations)
        {
            return validPlacementLocations.PickRandom();
        }

        public ShipBlueprint ChooseBlueprintToUpgrade(IEnumerable<ShipBlueprint> blueprints)
        {
            return blueprints.PickRandom();
        }

        public ShipPart ChooseUpgrade(IEnumerable<ShipPart> availableParts)
        {
            return availableParts.PickRandom();
        }

        public ShipPart ChoosePartToReplace(ShipBlueprint blueprint)
        {
            return blueprint.Parts.PickRandom();
        }

        public PopulationSquare ChooseColonizationLocation(List<PopulationSquare> validSquares)
        {
            return validSquares.PickRandom();
        }

        public PlayerShip ChooseShipToMove(IEnumerable<PlayerShip> ships)
        {
            throw new System.NotImplementedException();
        }

        public Sector ChooseShipDestination(PlayerShip ship, IList<Sector> validDestinations)
        {
            throw new System.NotImplementedException();
        }
    }
}