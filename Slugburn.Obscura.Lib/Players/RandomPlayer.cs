using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Ai;
using Slugburn.Obscura.Lib.Ai.Actions;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Players
{
    public class RandomPlayer : IAiPlayer
    {
        private readonly BlueprintGenerator _blueprintGenerator;
        private readonly IList<IBuilder> _builders;
        private readonly IActionDecision _actionDecision;

        public RandomPlayer(BlueprintGenerator blueprintGenerator, IList<IBuilder> builders, ShouldPassDecision actionDecision)
        {
            _blueprintGenerator = blueprintGenerator;
            _builders = builders;
            _actionDecision = actionDecision;
        }

        private Dictionary<ShipBlueprint, IList<ShipPart>> _idealBlueprints;

        public Faction Faction { get; set; }

        public bool ChooseToClaimSector(Sector sector)
        {
            return !Faction.SpendingInfluenceWillBankrupt();
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
            ValidActions = validActions;
            return _actionDecision.GetResult(this);
        }

        public IEnumerable<IAction> ValidActions { get; set; }
        public IList<BuildLocation> BuildList { get; set; }
        public Tech TechToResearch { get; set; }

        public void RotateSectorWormholes(Sector sector, int[] validFacings)
        {
            while (!sector.Wormholes.Intersect(validFacings).Any())
                sector.RotateClockwise();
        }

        public MapLocation ChooseSectorLocation(IEnumerable<MapLocation> availableLocations)
        {
            var locations = availableLocations.ToList();
            var closest = locations.Where(loc => loc.DistanceFromCenter == locations.Min(loc1 => loc1.DistanceFromCenter));
            return closest.PickRandom();
        }

        public Tech ChooseResearch(IEnumerable<Tech> availableTech)
        {
            if (!availableTech.Any(x=>x.Equals(TechToResearch)))
                throw new InvalidOperationException("Selected research technology is not valid.");
            return TechToResearch;
        }

        public IBuilder ChooseBuilder(IEnumerable<IBuilder> validBuilders)
        {
            var builder = BuildList.First().Builder;
            if (!validBuilders.Any(x=>x.Equals(builder)))
                throw new InvalidOperationException(string.Format("Building {0} is not valid.", builder.Name));
            return builder;
        }

        public Sector ChoosePlacementLocation(IBuildable built, List<Sector> validPlacementLocations)
        {
            var location = BuildList[0].Location;
            if (validPlacementLocations.All(x => x != location))
                throw new InvalidOperationException(string.Format("Placing {0} at {1} is not valid.", built.Name, location));
            BuildList.RemoveAt(0);
            return location;
        }

        public ShipBlueprint ChooseBlueprintToUpgrade(IEnumerable<ShipBlueprint> blueprints)
        {
            return GetUpgradeableBlueprints().FirstOrDefault();
        }

        private IEnumerable<ShipBlueprint> GetUpgradeableBlueprints()
        {
            return _idealBlueprints.Select(
                x => new {Blueprint = x.Key, Difference = _blueprintGenerator.RateBlueprint(x.Key, x.Value) - _blueprintGenerator.RateBlueprint(x.Key)})
                .Where(x => x.Difference >= 0.5m)
                .OrderBy(x => x.Difference)
                .Select(x=>x.Blueprint);
        }

        private void UpdateIdealBlueprints()
        {
            var blueprints = new[] {Faction.Interceptor, Faction.Cruiser, Faction.Dreadnought, Faction.Starbase};
            var partsPool = Faction.GetAvailableShipParts().ToList();
            _idealBlueprints = blueprints.ToDictionary(x=>x, x=>_blueprintGenerator.GetBestParts(x, partsPool));
        }

        public ShipPart ChoosePartToReplace(ShipBlueprint blueprint)
        {
            // Get a part that is not in the ideal template
            var difference = blueprint.Parts.Less(_idealBlueprints[blueprint]).ToArray();
            if (!difference.Any())
            {
                throw new Exception(String.Join(", ", blueprint.Parts) + " : " + String.Join(", ", _idealBlueprints[blueprint]));
            }
            return difference.OrderBy(x => x.Move).ThenBy(x=>x.Energy).First();
        }

        public ShipPart ChooseUpgrade(ShipBlueprint blueprint, IEnumerable<ShipPart> availableParts)
        {
            // Get a part that is in the ideal template but not the current
            return _idealBlueprints[blueprint].Less(blueprint.Parts).OrderByDescending(x=>x.Energy).First();
        }

        public PopulationSquare ChooseColonizationLocation(List<PopulationSquare> validSquares)
        {
            return validSquares.PickRandom();
        }

        public void HandleBankruptcy()
        {
            var ratio = Faction.TradeRatio;
            if (Faction.Science >= ratio || Faction.Material >= ratio)
            {
                if (Faction.Science > Faction.Material)
                    Faction.Science -= ratio;
                else
                    Faction.Material -= ratio;
                Faction.Money++;
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public ProductionType ChooseColonizationType(ProductionType productionType)
        {
            return productionType == ProductionType.Any ? ProductionType.Material : ProductionType.Science;
        }

        public PlayerShip ChooseShipToMove(IEnumerable<PlayerShip> ships)
        {
            return ships.PickRandom();
        }

        public Sector ChooseShipDestination(PlayerShip ship, IList<Sector> validDestinations)
        {
            return validDestinations.PickRandom();
        }

        public Sector RallyPoint { get; set; }
    }
}