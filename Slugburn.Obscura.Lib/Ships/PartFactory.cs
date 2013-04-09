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
            return new ShipPart {Name = "Ion Cannon", Energy = -1, Cannons = new[] {1}};
        }

        public static ShipPart NuclearDrive()
        {
            return new ShipPart {Name = "Nuclear Drive", Energy = -1, Initiative = 1, Move = 1};
        }

        public static ShipPart NuclearSource()
        {
            return new ShipPart {Name = "Nuclear Source", Energy = 3};
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