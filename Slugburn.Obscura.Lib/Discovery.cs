using System;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Messages;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib
{
    public class Discovery
    {
        private readonly Action<PlayerFaction> _onUse;
        public string Name { get; set; }

        public Discovery(string name, Action<PlayerFaction> onUse)
        {
            _onUse = onUse;
            Name = name;
        }

        public void Use(PlayerFaction faction)
        {
            _onUse(faction);
        }

        public override string ToString()
        {
            return Name;
        }

        public static Discovery Money
        {
            get
            {
                return new Discovery("+8 Money", f =>
                    {
                        f.Money += 8;
                        f.Log("\t+8 Money gained");
                    });
            }
        }

        public static Discovery Science
        {
            get
            {
                return new Discovery("+5 Science", f =>
                    {
                        f.Science += 5;
                        f.Log("\t+5 Science gained");
                    });
            }
        }

        public static Discovery Materials
        {
            get
            {
                return new Discovery("+6 Materials", f =>
                    {
                        f.Material += 6;
                        f.Log("+6 Materials gained");
                    });
            }
        }

        public static Discovery AncientTechnology
        {
            get { return new Discovery("Ancient Technology", ChooseLowestCostTech); }
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

        public static Discovery AncientCruiser
        {
            get
            {
                // get free cruiser
                return new Discovery("Ancient Cruiser", MakeAncientCruiser);
            }
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
            faction.SendMessage(new PartDiscovered(part));
//            faction.Game.GetAction<UpgradeAction>().UpgradeUsingDiscoveredPart(faction, part);
        }

        public static Discovery AxiomComputer
        {
            get
            {
                // +3 accuracy
                return new Discovery("Axiom Computer", f => UseDiscoveredPart(f, new AncientShipPart {Name = "Axiom Computer", Accuracy = 3}));
            }
        }

        public static Discovery HypergridSource
        {
            get
            {
                // +11 energy
                return new Discovery("Hypergrid Source", f => UseDiscoveredPart(f, new AncientShipPart { Name = "Hypergrid Source", Energy = 11 }));
            }
        }

        public static Discovery ShardHull
        {
            get
            {
                // 3 structure
                return new Discovery("Shard Hull", f => UseDiscoveredPart(f, new AncientShipPart { Name = "Shard Hull", Structure = 3 }));
            }
        }

        public static Discovery IonTurret
        {
            get
            {
                // 2 x 1 damage, -1 energy
                return new Discovery("Ion Turret", f => UseDiscoveredPart(f, new AncientShipPart { Name = "Ion Turret", Cannons = new[] { 1, 1 }, Energy = -1 }));
            }
        }

        public static Discovery ConformalDrive
        {
            get
            {
                // 4 move, 2 init, -2 energy
                return new Discovery("Conformal Drive",
                                     f => UseDiscoveredPart(f, new AncientShipPart { Name = "Conformal Drive", Move = 4, Initiative = 2, Energy = -2 }));
            }
        }

        public static Discovery FluxShield
        {
            get
            {
                // -3 deflection, -2 energy
                return new Discovery("Flux Shield", f => UseDiscoveredPart(f, new AncientShipPart { Name = "Flux Shield", Deflection = -3, Energy = -2 }));
            }
        }
    }
}