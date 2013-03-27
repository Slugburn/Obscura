using System.Collections.Generic;

namespace Slugburn.Obscura.Lib.Ships
{
    public class PartFactory
    {

        public static IEnumerable<ShipPart> GetBasicParts()
        {
            return new[]
                {
                    IonCannon(),
                    NuclearDrive(),
                    NuclearSource(),
                    Hull(),
                    ElectronComputer()
                };
        }

        public static ShipPart IonCannon()
        {
            return new ShipPart {Name = "Ion Cannon", Power = -1, Damage = new[] {1}};
        }

        public static ShipPart NuclearDrive()
        {
            return new ShipPart {Name = "Nuclear Drive", Power = -1, Initiative = 1, Move = 1};
        }

        public static ShipPart NuclearSource()
        {
            return new ShipPart {Name = "Nuclear Source", Power = 3};
        }

        public static ShipPart Hull()
        {
            return new ShipPart {Name = "Hull", Structure = 1};
        }

        public static ShipPart ElectronComputer()
        {
            return new ShipPart {Name = "Electron Computer", Accuracy = 1};
        }
    }
}