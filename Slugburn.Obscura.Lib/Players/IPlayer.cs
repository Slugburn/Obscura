using System.Collections.Generic;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;
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
        IBuilder ChooseBuilder(IEnumerable<IBuilder> validBuilders);
        Sector ChoosePlacementLocation(IBuildable built, List<Sector> validPlacementLocations);
        ShipBlueprint ChooseBlueprintToUpgrade(IEnumerable<ShipBlueprint> blueprints);
        ShipPart ChooseUpgrade(IEnumerable<ShipPart> availableParts);
        ShipPart ChoosePartToReplace(ShipBlueprint blueprint);
        PopulationSquare ChooseColonizationLocation(List<PopulationSquare> validSquares);
        PlayerShip ChooseShipToMove(IEnumerable<PlayerShip> ships);
        Sector ChooseShipDestination(PlayerShip ship, IList<Sector> validDestinations);
    }
}