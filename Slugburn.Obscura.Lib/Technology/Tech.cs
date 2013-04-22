using System;
using System.Linq;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Technology
{
    public class Tech
    {
        protected bool Equals(Tech other)
        {
            return string.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public string Name { get; set; }
        public int MinCost { get; set; }
        public int Cost { get; set; }
        public TechCategory Category { get; set; }

        public Tech(string name, int cost, int minCost, TechCategory category)
        {
            Name = name;
            MinCost = minCost;
            Cost = cost;
            Category = category;
        }

        protected Tech(){}

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Tech) obj);
        }

        public static Tech NeutronBombs
        {
            get { return new Tech("Neutron Bombs", 2, 2, TechCategory.Military); }
        }

        public static Tech Starbase
        {
            get { return new Tech("Starbase", 4, 3, TechCategory.Military); }
        }

        public static Tech PlasmaCannon
        {
            get { return new PartTech("Plasma Cannon", 6, 4, TechCategory.Military, PartType.Cannon, () => new ShipPart {Cannons = new[] {2}, Energy = -2}); }
        }

        public static Tech PhaseShield
        {
            get { return new PartTech("Phase Shield", 8, 5, TechCategory.Military, PartType.Shield, () => new ShipPart {Deflection = -2, Energy = -1}); }
        }

        public static Tech AdvancedMining
        {
            get { return new Tech("Advanced Mining", 10, 6, TechCategory.Military); }
        }

        public static Tech TachyonSource
        {
            get { return new PartTech("Tachyon Source", 12, 6, TechCategory.Military, PartType.Source, () => new ShipPart {Energy = 9}); }
        }

        public static Tech PlasmaMissile
        {
            get { return new PartTech("Plasma Missile", 14, 7, TechCategory.Military, PartType.Missile, () => new ShipPart {Missiles = new[] {2, 2}}); }
        }

        public static Tech GluonComputer
        {
            get { return new PartTech("Gluon Computer", 16, 8, TechCategory.Military, PartType.Computer, () => new ShipPart {Accuracy = 3, Initiative = 2, Energy = -2}); }
        }

        public static Tech GaussShield
        {
            get { return new PartTech("Gauss Shield", 2, 2, TechCategory.Grid, PartType.Shield, () => new ShipPart {Deflection = -1}); }
        }

        public static Tech ImprovedHull
        {
            get { return new PartTech("Improved Hull", 4, 3, TechCategory.Grid, PartType.Hull, () => new ShipPart {Structure = 2}); }
        }

        public static Tech FusionSource
        {
            get { return new PartTech("Fusion Source", 6, 4, TechCategory.Grid, PartType.Hull,  () => new ShipPart {Energy = 6}); }
        }

        public static Tech PositronComputer
        {
            get { return new PartTech("Positron Computer", 8, 5, TechCategory.Grid, PartType.Computer, () => new ShipPart {Accuracy = 2, Initiative = 1, Energy = -1}); }
        }

        public static Tech AdvancedEconomy
        {
            get { return new Tech("Advanced Economy", 10, 6, TechCategory.Grid); }
        }

        public static Tech TachyonDrive
        {
            get { return new PartTech("Tachyon Drive", 12, 6, TechCategory.Grid, PartType.Drive, () => new ShipPart {Move = 3, Initiative = 3, Energy = -3}); }
        }

        public static Tech AntimatterCannon
        {
            get { return new PartTech("Antimatter Cannon", 14, 7, TechCategory.Grid, PartType.Cannon, () => new ShipPart {Cannons = new[] {4}, Energy = -4}); }
        }

        public static Tech QuantumGrid
        {
            get { return new EffectTech("Quantum Grid", 16, 8, TechCategory.Grid, faction => faction.Influence += 2); }
        }

        public static Tech Nanorobots
        {
            get { return new EffectTech("Nanorobots", 2, 2, TechCategory.Nano, faction=>faction.BuildCount++); }
        }

        public static Tech FusionDrive
        {
            get { return new PartTech("Fusion Drive", 4, 3, TechCategory.Nano, PartType.Drive, () => new ShipPart {Move = 2, Initiative = 2, Energy = -2}); }
        }

        public static Tech AdvancedRobotics
        {
            get { return new EffectTech("Advanced Robotics", 6, 4, TechCategory.Nano, faction => faction.Influence++); }
        }

        public static Tech Orbital
        {
            get { return new Tech("Orbital", 8, 5, TechCategory.Nano); }
        }

        public static Tech AdvancedLabs
        {
            get { return new Tech("Advanced Labs", 10, 6, TechCategory.Nano); }
        }

        public static Tech Monolith
        {
            get { return new Tech("Monolith", 12, 6, TechCategory.Nano); }
        }

        public static Tech ArtifactKey
        {
            get { return new EffectTech("Artifact Key", 14, 7, TechCategory.Nano, UseArtifactKey); }
        }

        private static void UseArtifactKey(Faction faction)
        {
            // TODO: Give choices
            faction.Money += faction.Sectors.Count(s=>s.HasArtifact) * 5;
        }

        public static Tech WormholeGenerator
        {
            get { return new Tech("Wormhole Generator", 16, 8, TechCategory.Nano); }
        }
    }
}
