using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Technology
{
    public class Tech
    {
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

        public static Tech NeutronBombs()
        {
            return new Tech("Neutron Bombs", 2, 2, TechCategory.Military);
        }

        public static Tech Starbase()
        {
            return new Tech("Starbase", 4, 3, TechCategory.Military);
        }

        public static Tech PlasmaCannon()
        {
            return new PartTech("Plasma Cannon", 6, 4, TechCategory.Military, () => new ShipPart {Damage = new[] {2}, Power = -2});
        }

        public static Tech PhaseShield()
        {
            return new PartTech("Phase Shield", 8, 5, TechCategory.Military, () => new ShipPart {Deflection = -2, Power = -1});
        }

        public static Tech AdvancedMining()
        {
            return new Tech("Advanced Mining", 10, 6, TechCategory.Military);
        }

        public static Tech TachyonSource()
        {
            return new PartTech("Tachyon Source", 12, 6, TechCategory.Military, () => new ShipPart {Power = 9});
        }

        public static Tech PlasmaMissile()
        {
            return new PartTech("Plasma Missile", 14, 7, TechCategory.Military, () => new ShipPart {Damage = new[] {2, 2}, FirstStrike = true});
        }

        public static Tech GluonComputer()
        {
            return new PartTech("Gluon Computer", 16, 8, TechCategory.Military, () => new ShipPart {Accuracy = 3, Initiative = 2, Power = -2});
        }
        
        public static Tech GaussShield()
        {
            return new PartTech("Gauss Shield", 2, 2, TechCategory.Grid, () => new ShipPart {Deflection = -1});
        }

        public static Tech ImprovedHull()
        {
            return new PartTech("Improved Hull", 4, 3, TechCategory.Grid, () => new ShipPart {Structure = 2});
        }

        public static Tech FusionSource()
        {
            return new PartTech("Fusion Source", 6, 4, TechCategory.Grid, () => new ShipPart {Power = 6});
        }

        public static Tech PositronComputer()
        {
            return new PartTech("Positron Computer", 8, 5, TechCategory.Grid, () => new ShipPart {Accuracy = 2, Initiative = 1, Power = -1});
        }

        public static Tech AdvancedEconomy()
        {
            return new Tech("Advanced Economy", 10, 6, TechCategory.Grid);
        }

        public static Tech TachyonDrive()
        {
            return new PartTech("Tachyon Drive", 12, 6, TechCategory.Grid, () => new ShipPart {Move = 3, Initiative = 3, Power = -3});
        }

        public static Tech AntimatterCannon()
        {
            return new PartTech("Antimatter Cannon", 14, 7, TechCategory.Grid, () => new ShipPart {Damage = new[] {4}, Power = -4});
        }

        public static Tech QuantumGrid()
        {
            return new Tech("Quantum Grid", 16, 8, TechCategory.Grid);
        }

        public static Tech Nanorobots()
        {
            return new Tech("Nanorobots",2,2,TechCategory.Nano);
        }

        public static Tech FusionDrive()
        {
            return new PartTech("Fusion Drive", 4, 3, TechCategory.Nano, () => new ShipPart {Move = 2, Initiative = 2, Power = -2});
        }

        public static Tech AdvancedRobotics()
        {
            return new Tech("Advanced Robotics", 6, 4, TechCategory.Nano);
        }

        public static Tech Orbital()
        {
            return new Tech("Orbital", 8, 5, TechCategory.Nano);
        }

        public static Tech AdvancedLabs()
        {
            return new Tech("Advanced Labs", 10, 6, TechCategory.Nano);
        }

        public static Tech Monolith()
        {
            return new Tech("Monolith", 12, 6, TechCategory.Nano);
        }

        public static Tech ArtifactKey()
        {
            return new Tech("Artifact Key", 14, 7, TechCategory.Nano);
        }

        public static Tech WormholeGenerator()
        {
            return new Tech("Wormhole Generator", 16, 8, TechCategory.Nano);
        }
    }
}