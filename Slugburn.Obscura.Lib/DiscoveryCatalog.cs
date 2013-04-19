using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Ships;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib
{
    public class DiscoveryCatalog
    {
        public static IEnumerable<Discovery> GetTiles()
        {
            return new[]
                       {
                           Money().Repeat(3),
                           Science().Repeat(3),
                           Materials().Repeat(3),
                           AncientTechnology().Repeat(3),
                           AncientCruiser().Repeat(3),
                           new[]
                               {
                                   AxiomComputer(),
                                   HypergridSource(),
                                   ShardHull(),
                                   IonTurret(),
                                   ConformalDrive(),
                                   FluxShield()
                               }
                       }
                .SelectMany(x => x);
        }

        private static Discovery Money()
        {
            return new Discovery("+8 Money", f => { f.Money += 8; f.Log("\t+8 Money gained");});
        }

        private static Discovery Science()
        {
            return new Discovery("+5 Science", f => { f.Science += 5; f.Log("\t+5 Science gained"); });
        }

        private static Discovery Materials()
        {
            return new Discovery("+6 Materials", f => { f.Material += 6; f.Log("+6 Materials gained"); });
        }

        private static Discovery AncientTechnology()
        {
            return new Discovery("Ancient Technology", ChooseLowestCostTech);
        }

        private static void ChooseLowestCostTech(PlayerFaction faction)
        {
            // choose lowest cost unknown tech
            var available = faction.UnknownTech().ToArray();
            var minCost = available.Min(x => x.Cost);
            var techs = from tech in available
                        where tech.Cost == minCost
                        select tech;
            var chosen = faction.Player.ChooseDiscoveredTech(techs);
            var research = faction.Game.GetAction<ResearchAction>();
            research.ClaimTech(faction, chosen);
            faction.Log("\t{0} claimed", chosen);
        }

        private static Discovery AncientCruiser()
        {
            // get free cruiser
            return new Discovery("Ancient Cruiser", MakeAncientCruiser);
        }

        private static void MakeAncientCruiser(PlayerFaction faction)
        {
            var cruiser = faction.CreateShip(faction.Cruiser);
            var sector = faction.Player.ChoosePlacementLocation(cruiser, faction.Sectors);
            cruiser.Place(sector);
            faction.Log("\tDiscovered cruiser placed at {0}", sector);
        }

        private static void UseDiscoveredPart(PlayerFaction faction, ShipPart part)
        {
            faction.Game.GetAction<UpgradeAction>().UpgradeUsingDiscoveredPart(faction, part);
        }

        private static Discovery AxiomComputer()
        {
            // +3 accuracy
            return new Discovery("Axiom Computer", f => UseDiscoveredPart(f, new ShipPart {Name = "Axiom Computer", Accuracy = 3}));
        }

        private static Discovery HypergridSource()
        {
            // +11 energy
            return new Discovery("Hypergrid Source", f => UseDiscoveredPart(f, new ShipPart {Name = "Hypergrid Source", Energy = 11}));
        }

        private static Discovery ShardHull()
        {
            // 3 structure
            return new Discovery("Shard Hull", f => UseDiscoveredPart(f, new ShipPart {Name = "Shard Hull", Structure = 3}));
        }

        private static Discovery IonTurret()
        {
            // 2 x 1 damage, -1 energy
            return new Discovery("Ion Turret", f => UseDiscoveredPart(f, new ShipPart {Name = "Ion Turret", Cannons = new[] {1, 1}, Energy = -1}));
        }

        private static Discovery ConformalDrive()
        {
            // 4 move, 2 init, -2 energy
            return new Discovery("Conformal Drive",
                                 f => UseDiscoveredPart(f, new ShipPart {Name = "Conformal Drive", Move = 4, Initiative = 2, Energy = -2}));
        }

        private static Discovery FluxShield()
        {
            // -3 deflection, -2 energy
            return new Discovery("Flux Shield", f => UseDiscoveredPart(f, new ShipPart {Name = "Flux Shield", Deflection = -3, Energy = -2}));
        }


    }
}