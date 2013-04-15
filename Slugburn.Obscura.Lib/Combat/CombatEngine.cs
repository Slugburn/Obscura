using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Players;
using Slugburn.Obscura.Lib.Ships;

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
                var damageRolls = weapons.Select(d => new DamageRoll {Damage = d, Roll = _random.Next(1, 6 + 1)}).ToArray();
                var targets = g.EnemyFaction.Ships.Select(x => new Target {Ship = x, Number = Math.Min(6, 6 - (g.Accuracy + x.Profile.Deflection))}).ToArray();
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
                        groups.Single(grp => grp.ShipType == ship.ShipType).Ships.Remove(ship);
                        // Remove from map
                        ship.Sector.Ships.Remove(ship);
                        // Remove from faction list
                        var playerFaction = ship.Faction as PlayerFaction;
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

        private static int GetFactionPriority(Sector sector, IFaction faction, IEnumerable<Ship> ships)
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

        public IFaction Faction { get; set; }

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
        public IFaction Faction { get; set; }
        public List<Ship> Ships { get; set; }
        public int Priority { get; set; }
        public bool HasInitiative { get; set; }

        public CombatFaction(IFaction faction, IEnumerable<Ship> ships, int priority)
        {
            Faction = faction;
            Ships = ships.ToList();
            Priority = priority;
        }

    }

}
