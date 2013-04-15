using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Ai;
using Slugburn.Obscura.Lib.Ai.Actions;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Combat;
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
        private readonly IActionDecision _actionDecision;
        private readonly ILog _log;

        public RandomPlayer(BlueprintGenerator blueprintGenerator, ShouldPassDecision actionDecision, ILog log)
        {
            _blueprintGenerator = blueprintGenerator;
            _actionDecision = actionDecision;
            _log = log;
        }

        private Dictionary<ShipBlueprint, IList<ShipPart>> _idealBlueprints;

        public PlayerFaction Faction { get; set; }

        public bool ChooseToClaimSector(Sector sector)
        {
            var sectoryProducesMoney = sector.Squares.Any(x => x.ProductionType == ProductionType.Money
                                                               && (!x.Advanced || Faction.HasTechnology(Tech.AdvancedEconomy)));
            return sectoryProducesMoney || !Faction.SpendingInfluenceWillBankrupt();
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
        public IList<ShipMove> MoveList { get; set; }
        public IList<BuildLocation> BuildList { get; set; }
        public Tech TechToResearch { get; set; }
        public IList<BlueprintUpgrade> UpgradeList { get; set; }

        public IList<ShipPart> GetIdealPartList(ShipBlueprint blueprint)
        {
            if (_idealBlueprints==null)
                UpdateIdealBlueprints();
            return _idealBlueprints[blueprint];
        }

        public void RotateSectorWormholes(Sector sector, int[] validFacings)
        {
            while (!sector.Wormholes.Intersect(validFacings).Any())
                sector.RotateClockwise();
        }

        public MapLocation ChooseSectorLocation(IEnumerable<MapLocation> availableLocations)
        {
            var locations = availableLocations.ToList();
            var closestDistance = locations.Min(loc1 => loc1.DistanceFromCenter);
            var closest = locations.Where(loc => loc.DistanceFromCenter == closestDistance);
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
            if (!BuildList.Any())
                return null;
            var builder = BuildList.First().Builder;
            if (!validBuilders.Any(x=>x.Equals(builder)))
                throw new InvalidOperationException(string.Format("Building {0} is not valid.", builder.Name));
            return builder;
        }

        public Sector ChoosePlacementLocation(IBuildable built, List<Sector> validPlacementLocations)
        {
            var location = BuildList[0].Location;
            if (validPlacementLocations.All(x => x != location))
                throw new InvalidOperationException(string.Format("Placing {0} at {1} is not valid.", built, location));
            BuildList.RemoveAt(0);
            return location;
        }

        public ShipBlueprint ChooseBlueprintToUpgrade(IEnumerable<ShipBlueprint> blueprints)
        {
            var blueprint = UpgradeList[0].Blueprint;
            if (blueprints.All(x => x != blueprint))
                throw new InvalidOperationException(string.Format("{0} is not valid to upgrade", blueprint));
            return blueprint;
        }

        private void UpdateIdealBlueprints()
        {
            var blueprints = new[] {Faction.Interceptor, Faction.Cruiser, Faction.Dreadnought, Faction.Starbase};
            var partsPool = Faction.GetAvailableShipParts().ToList();
            _idealBlueprints = blueprints.ToDictionary(x=>x, x=>_blueprintGenerator.GetBestParts(x, partsPool));
            _log.Log("{0} updates ideal blueprints:", Faction);
            _idealBlueprints.Each(kvp => _log.Log("\t{0}: {1}", kvp.Key.Name, kvp.Value.ListToString()));
        }

        public ShipPart ChoosePartToReplace(ShipBlueprint blueprint)
        {
            return UpgradeList[0].Replace;
        }

        public ShipPart ChooseUpgrade(ShipBlueprint blueprint)
        {
            return UpgradeList[0].Upgrade;
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
            return MoveList.Count > 0 ? MoveList[0].Ship : null;
        }

        public Sector ChooseShipDestination(PlayerShip ship, IList<Sector> validDestinations)
        {
            var sector = MoveList[0].Destination;
            MoveList.RemoveAt(0);
            if (validDestinations.All(x => x != sector))
                throw new InvalidOperationException(string.Format("Moving {0} to {1} is not valid.", ship, sector));
            return sector;
        }

        public IEnumerable<Target> ChooseDamageDistribution(IEnumerable<DamageRoll> damageRolls, IEnumerable<Target> targets)
        {
            return PickDamageDistribution(damageRolls, targets);
        }

        public static IEnumerable<Target> PickDamageDistribution(IEnumerable<DamageRoll> hits, IEnumerable<Target> targets)
        {
            var rolls = hits.ToList();
            var myTargets = targets.ToArray();
            var liveTargets = targets.ToList();
            while (rolls.Any())
            {
                // find largest target that can be destroyed
                var largestDestroyable =
                    liveTargets.Where(t => t.Ship.RemainingStructure < rolls.Where(r => r.Roll >= t.Number).Sum(r => r.Damage))
                        .OrderByDescending(t => t.Ship.Rating)
                        .FirstOrDefault();
                if (largestDestroyable != null)
                {
                    var necessaryDamage = largestDestroyable.Ship.RemainingStructure + 1;
                    while (necessaryDamage > 0)
                    {
                        var hitsAgainstTarget = rolls.Where(r => r.Roll >= largestDestroyable.Number).ToArray();
                        // Use largest damage roll to that doesn't overkill the target, if available. Otherwise use smallest damage roll.
                        var rollToUse = hitsAgainstTarget.Where(r => r.Damage <= necessaryDamage).OrderByDescending(r => r.Damage).FirstOrDefault();
                        rollToUse = rollToUse ?? hitsAgainstTarget.OrderBy(r => r.Damage).First();
                        rolls.Remove(rollToUse);
                        largestDestroyable.Damage += rollToUse.Damage;
                        necessaryDamage -= rollToUse.Damage;
                    }
                    liveTargets.Remove(largestDestroyable);
                }
                else
                {
                    // allocate remaining hits to largest target
                    foreach (var roll in rolls)
                    {
                        var largestHitable = liveTargets.Where(t => t.Number <= roll.Roll).OrderByDescending(t => t.Ship.Rating).FirstOrDefault();
                        if (largestHitable != null)
                            largestHitable.Damage += roll.Damage;
                    }
                    break;
                }
            }
            return myTargets.Where(x=>x.Damage > 0);
        }

        public void AfterAction(IAction chosenAction)
        {
            if (chosenAction is ResearchAction)
                UpdateIdealBlueprints();
        }

        public void AfterUpgradeCompleted()
        {
            UpgradeList.RemoveAt(0);
        }

        public InfluenceDirection ChooseInfluenceDirection()
        {
            return InfluenceDirection.Place;
        }

        public Sector ChooseInfluencePlacementLocation(IEnumerable<Sector> validLocations)
        {
            if (!InfluenceList.Any())
                return null;
            var location = InfluenceList[0].Location;
            InfluenceList.RemoveAt(0);
            return location;
        }

        public IList<InfluenceLocation> InfluenceList { get; set; }

        public Sector RallyPoint { get; set; }
    }
}