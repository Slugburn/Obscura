using System;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Messages;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib
{
    public class Discovery
    {
        private readonly Action<DiscoveryUsage> _onUse;
        public string Name { get; set; }

        public Discovery(string name, Action<DiscoveryUsage> onUse)
        {
            _onUse = onUse;
            Name = name;
        }

        public void Use(DiscoveryUsage discoveryUsage)
        {
            _onUse(discoveryUsage);
        }

        public override string ToString()
        {
            return Name;
        }

        public static Discovery Money
        {
            get
            {
                return new Discovery("+8 Money", x =>
                    {
                        x.Faction.Money += 8;
                        x.Faction.Log("\t+8 Money gained");
                    });
            }
        }

        public static Discovery Science
        {
            get
            {
                return new Discovery("+5 Science", x =>
                    {
                        x.Faction.Science += 5;
                        x.Faction.Log("\t+5 Science gained");
                    });
            }
        }

        public static Discovery Materials
        {
            get
            {
                return new Discovery("+6 Materials", x =>
                    {
                        x.Faction.Material += 6;
                        x.Faction.Log("+6 Materials gained");
                    });
            }
        }

        public static Discovery AncientTechnology
        {
            get { return new Discovery("Ancient Technology", ChooseLowestCostTech); }
        }

        private static void ChooseLowestCostTech(DiscoveryUsage usage)
        {
            var faction = usage.Faction;
            // choose lowest cost unknown tech
            var available = faction.UnknownTech().ToArray();
            var minCost = available.Min(x => x.Cost);
            var techs = from tech in available
                        where tech.Cost == minCost
                        select tech;
            var chosen = faction.Player.ChooseDiscoveredTech(techs);
            var research = faction.GetAction<ResearchAction>();
            research.ClaimTech(faction, chosen);
            faction.Log("\t{0} claimed", chosen);
        }

        public static Discovery AncientCruiser
        {
            get
            {
                // get free cruiser
                return new Discovery("Ancient Cruiser", MakeAncientCruiser);
            }
        }

        private static void MakeAncientCruiser(DiscoveryUsage usage)
        {
            var faction = usage.Faction;
            var cruiser = faction.CreateShip(faction.Cruiser);
            var sector = usage.DiscoveredIn;
            cruiser.Place(sector);
            faction.Log("\tAncient cruiser discovered in {0}", sector);
        }

        private static void UseDiscoveredPart(Faction faction, ShipPart part)
        {
            faction.SendMessage(new PartDiscovered(part));
//            faction.Game.GetAction<UpgradeAction>().UpgradeUsingDiscoveredPart(faction, part);
        }

        public static Discovery AxiomComputer
        {
            get
            {
                // +3 accuracy
                return new Discovery("Axiom Computer", x => UseDiscoveredPart(x.Faction, new AncientShipPart {Name = "Axiom Computer", Accuracy = 3}));
            }
        }

        public static Discovery HypergridSource
        {
            get
            {
                // +11 energy
                return new Discovery("Hypergrid Source", x => UseDiscoveredPart(x.Faction, new AncientShipPart { Name = "Hypergrid Source", Energy = 11 }));
            }
        }

        public static Discovery ShardHull
        {
            get
            {
                // 3 structure
                return new Discovery("Shard Hull", x => UseDiscoveredPart(x.Faction, new AncientShipPart { Name = "Shard Hull", Structure = 3 }));
            }
        }

        public static Discovery IonTurret
        {
            get
            {
                // 2 x 1 damage, -1 energy
                return new Discovery("Ion Turret", x =>
                                                       {
                                                           UseDiscoveredPart(x.Faction,
                                                                             new AncientShipPart {Name = "Ion Turret", Cannons = new[] {1, 1}, Energy = -1});
                                                       });
            }
        }

        public static Discovery ConformalDrive
        {
            get
            {
                // 4 move, 2 init, -2 energy
                return new Discovery("Conformal Drive",
                                     x =>
                                         {
                                             UseDiscoveredPart(x.Faction, new AncientShipPart {Name = "Conformal Drive", Move = 4, Initiative = 2, Energy = -2});
                                         });
            }
        }

        public static Discovery FluxShield
        {
            get
            {
                // -3 deflection, -2 energy
                return new Discovery("Flux Shield", x =>
                                                        {
                                                            UseDiscoveredPart(x.Faction,
                                                                              new AncientShipPart {Name = "Flux Shield", Deflection = -3, Energy = -2});
                                                        });
            }
        }
    }
}