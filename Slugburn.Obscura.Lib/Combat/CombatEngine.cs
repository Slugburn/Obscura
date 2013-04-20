using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Players;
using Slugburn.Obscura.Lib.Ships;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Combat
{
    public class CombatEngine
    {
        private readonly Random _random;
        private readonly ILog _log;

        public CombatEngine(Random random, ILog log)
        {
            _random = random;
            _log = log;
        }

        public void ResolveCombatPhase(IList<Sector> sectors)
        {
            var combatSectors = sectors
                .Where(s => s.Ships.GroupBy(ship => ship.Faction).Count() > 1)
                .OrderByDescending(sector => sector.Id)
                .ToList();
            foreach (var sector in combatSectors)
            {
                ResolveSectorCombat(sector);
                var winner = sector.Ships.Select(ship => ship.Faction).Distinct().SingleOrDefault();
                if (winner != null)
                    _log.Log("{0} wins combat.", winner);
            }
            sectors.Each(ResolveBombing);
            ClaimSectors(sectors);
        }

        private static void ClaimSectors(IEnumerable<Sector> sectors)
        {
            sectors.Where(x => x.Owner == null && x.Ships.Any(ship => ship is PlayerShip))
                .Each(x =>
                          {
                              var faction = x.Ships.Select(ship => ship.Faction).Cast<Faction>().First();
                              if (faction.Player.ChooseToClaimSector(x))
                                  faction.ClaimSector(x);
                          });
        }

        public void ResolveSectorCombat(Sector sector)
        {
            _log.Log("Combat begins in {0}", sector);
            while (true)
            {
                var combatFactions = sector.Ships.GroupBy(s => s.Faction).Select(g => new CombatFaction(g.Key, g, GetFactionPriority(sector, g.Key, g))).ToList();
                // continue until sector is uncontested
                if (combatFactions.Count < 2)
                    break;
                var currentPair = combatFactions.OrderByDescending(f => f.Priority).Take(2).ToArray();
                var cf1 = currentPair[0];
                var cf2 = currentPair[1];
                cf2.HasInitiative = true;
                ResolveFactionCombat(cf1, cf2);
            }
        }

        public void ResolveBombing(Sector sector)
        {
            if (sector.Owner == null || sector.Ships.All(x => x.Faction == sector.Owner)) 
                return;
            var attacker = sector.Ships.Select(x => x.Faction).Cast<Faction>().Distinct().Single();
            var populatedSquares = sector.Squares.Where(x => x.Owner != null).ToArray();
            if (!populatedSquares.Any())
            {
                ConquerSector(sector);
            }
            else if (attacker.HasTechnology(Tech.NeutronBombs))
            {
                _log.Log("{0} destroys all population in {1} with Neutron Bombs", attacker, sector);
                ConquerSector(sector);
            }
            else
            {
                var damage = sector.Ships.Sum(ship => ship.Cannons.Sum(x => RollDamageDie() >= GetTargetNumber(ship.Accuracy, 0) ? x : 0));
                if (damage == 0)
                {
                    _log.Log("{0}'s bombing in {1} is ineffective", attacker, sector);
                    return;
                }
                if (damage >= populatedSquares.Length)
                {
                    _log.Log("{0} destroys all population in {1} with bombing", attacker, sector);
                    ConquerSector(sector);
                }
                else
                {
                    var destroyedSquares = attacker.Player.ChoosePopulationToDestroy(sector, populatedSquares, damage).ToArray();
                    if (destroyedSquares.Length != damage || destroyedSquares.Except(populatedSquares).Any())
                        throw new InvalidOperationException("An invalid list of population to destroy was selected by the player.");
                    DestroyPopulation(destroyedSquares);
                }
            }
        }

        private void ConquerSector(Sector sector)
        {
            var populatedSquares = sector.Squares.Where(x => x.Owner != null).ToArray();
            DestroyPopulation(populatedSquares);
            sector.Owner.RelinquishSector(sector);
        }

        private void DestroyPopulation(PopulationSquare[] squares)
        {
            foreach (var square in squares)
            {
                var prodType = square.ProductionType;
                if (prodType == ProductionType.Orbital || prodType == ProductionType.Any)
                    prodType = square.Owner.Player.ChooseGraveyard(prodType);
                square.Owner.Graveyard[prodType]++;
            }
            squares.Each(x => x.Owner = null);
        }

        private void ResolveFactionCombat(CombatFaction faction1, CombatFaction faction2)
        {
            _log.Log("Combat between {0} and {1} begins", faction1.Faction, faction2.Faction);
            // Fire missiles first
            var shipGroups = CreateShipGroups(faction1, faction2);
            shipGroups.Each(g => _log.Log("{0}x {1} {2}", g.Ships.Count, g.Faction, g.ShipType));
            CompleteCombatRound(faction1, faction2, shipGroups, true);
            while (faction1.Ships.Count > 0 && faction2.Ships.Count > 0)
            {
                var groups = CreateShipGroups(faction1, faction2);
                CompleteCombatRound(faction1, faction2, groups, false);
            }
            // Repair damage to all surviving ships
            faction1.Ships.Each(x => x.Damage = 0);
            faction2.Ships.Each(x => x.Damage = 0);
        }

        private void CompleteCombatRound(CombatFaction faction1, CombatFaction faction2, List<ShipGroup> groups, bool missiles)
        {
            foreach (var g in groups)
            {
                if (faction1.Ships.Count == 0 || faction2.Ships.Count == 0)
                    break;
                if (g.Ships.Count == 0)
                    continue;
                var weapons = missiles ? g.Missiles : g.Cannons;
                if (!weapons.Any())
                    continue;
                var damageRolls = weapons.Select(d => new DamageRoll {Damage = d, Roll = RollDamageDie()}).ToArray();
                _log.Log("\t\t{0} rolls: {1}", g.Ships.First(), String.Join(" ", damageRolls.Select(x=>x.Roll.ToString()).OrderByDescending(x => x)));
                var targets = g.EnemyFaction.Ships.Select(x => new Target {Ship = x, Number = GetTargetNumber(g.Accuracy, x.Profile.Deflection)}).ToArray();
                var hits = damageRolls.Where(x => x.Roll >= targets.Min(t => t.Number));
                var damageDistribution = GetDamageDistribution(g, hits, targets).ToArray();
                foreach (var target in damageDistribution)
                {
                    var ship = target.Ship;
                    ship.Damage += target.Damage;
                    if (ship.Damage > ship.Profile.Structure)
                    {
                        _log.Log("\t{0} damage from {1} {2} destroys {3} {4}", target.Damage, g.Faction, g.Ships.First(), g.EnemyFaction.Faction, ship);
                        // Remove from enemy faction list
                        g.EnemyFaction.Ships.Remove(ship);
                        // Remove from ship group
                        groups.Single(grp => grp.Faction == g.EnemyFaction.Faction && grp.ShipType == ship.ShipType).Ships.Remove(ship);
                        // Remove from map
                        ship.Sector.Ships.Remove(ship);
                        // Remove from faction list
                        var playerFaction = ship.Faction as Faction;
                        if (playerFaction != null)
                            playerFaction.Ships.Remove((PlayerShip) ship);
                    }
                    else
                    {
                        _log.Log("\t{0} damage from {1} {2} damages {3} {4}", target.Damage, g.Faction, g.Ships.First(), g.EnemyFaction.Faction, ship);
                    }
                }
            }
        }

        private int RollDamageDie()
        {
            return _random.Next(1, 6 + 1);
        }

        private static int GetTargetNumber(int accuracy, int deflection)
        {
            return Math.Min(6, 6 - (accuracy + deflection));
        }

        private static IEnumerable<Target> GetDamageDistribution(ShipGroup shipGroup, IEnumerable<DamageRoll> hits, IEnumerable<Target> targets)
        {
            return shipGroup.Faction.ChooseDamageDistribution(hits, targets);
        }

        private List<ShipGroup> CreateShipGroups(CombatFaction faction1, CombatFaction faction2)
        {
            return faction1.Ships.GroupBy(x => x.ShipType).Select(g => CreateShipGroup(faction1, g.Key, g, faction2, faction1.HasInitiative))
                .Concat(faction2.Ships.GroupBy(x => x.ShipType).Select(g => CreateShipGroup(faction2, g.Key, g, faction1, faction2.HasInitiative)))
                .OrderByDescending(g => g.Initiative)
                .ToList();
        }

        private ShipGroup CreateShipGroup(CombatFaction myFaction, ShipType shipType, IEnumerable<Ship> ships, CombatFaction enemyFaction, bool hasInitiative)
        {
            var shipList = ships.ToList();
            return new ShipGroup
            {
                Faction = myFaction.Faction,
                ShipType = shipType,
                Ships = shipList,
                EnemyFaction = enemyFaction,
                Initiative = shipList.First().Profile.Initiative * 2 + (hasInitiative ? 1 : 0)
            };
        }

        private static int GetFactionPriority(Sector sector, IShipOwner faction, IEnumerable<Ship> ships)
        {
            return sector.Owner == faction ? -1 : ships.Min(ship => sector.Ships.IndexOf(ship));
        }


    }

    internal class ShipGroup
    {
        public ShipType ShipType { get; set; }

        public List<Ship> Ships { get; set; }

        public CombatFaction EnemyFaction { get; set; }

        public int Initiative { get; set; }

        public IShipOwner Faction { get; set; }

        public int[] Cannons
        {
            get { return Ships.SelectMany(x => x.Profile.Cannons).ToArray(); }
        }

        public int Accuracy
        {
            get { return Ships.First().Profile.Accuracy; }
        }

        public int[] Missiles
        {
            get { return Ships.First().Profile.Missiles ?? new int[0]; }
        }
    }

    internal class CombatFaction
    {
        public IShipOwner Faction { get; set; }
        public List<Ship> Ships { get; set; }
        public int Priority { get; set; }
        public bool HasInitiative { get; set; }

        public CombatFaction(IShipOwner faction, IEnumerable<Ship> ships, int priority)
        {
            Faction = faction;
            Ships = ships.ToList();
            Priority = priority;
        }

    }

}
