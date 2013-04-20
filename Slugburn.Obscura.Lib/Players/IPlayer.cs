using System.Collections.Generic;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Combat;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Players
{
    public interface IPlayer
    {
        void SetFaction(Faction faction);
        Faction Faction { get; set; }

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
        ShipPart ChoosePartToReplace(ShipBlueprint blueprint, IEnumerable<ShipPart> validReplacements);
        ShipPart ChooseUpgrade(ShipBlueprint blueprint, IEnumerable<ShipPart> validUpgrades);
        PopulationSquare ChooseColonizationLocation(List<PopulationSquare> validSquares);
        void HandleBankruptcy();
        ProductionType ChooseColonizationType(ProductionType productionType);
        PlayerShip ChooseShipToMove(IEnumerable<PlayerShip> ships);
        IList<Sector> ChooseShipPath(PlayerShip ship, IList<Sector> validDestinations);
        IEnumerable<Target> ChooseDamageDistribution(IEnumerable<DamageRoll> damageRolls, IEnumerable<Target> targets);
        InfluenceDirection ChooseInfluenceDirection();
        Sector ChooseInfluencePlacementLocation(IEnumerable<Sector> validLocations);
        IEnumerable<PopulationSquare> ChoosePopulationToDestroy(Sector sector, PopulationSquare[] populatedSquares, int damage);
        ProductionType ChooseProductionToAbandon(ProductionType prodType);
        ProductionType ChooseGraveyard(ProductionType prodType);
        Tech ChooseDiscoveredTech(IEnumerable<Tech> techs);
    }
}