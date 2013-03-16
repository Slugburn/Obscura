using System.Collections.Generic;

namespace Slugburn.Obscura.Lib
{
    public abstract class PlayerFactory : IPlayerFactory
    {
        private readonly string _name;
        private readonly int _startingMoney;
        private readonly int _startingScience;
        private readonly int _startingMaterials;

        protected PlayerFactory(string name, int startingMoney, int startingScience, int startingMaterials)
        {
            _name = name;
            _startingMoney = startingMoney;
            _startingScience = startingScience;
            _startingMaterials = startingMaterials;
        }

        public virtual Player Create()
        {
            return new Player
            {
                Name = _name,
                Money = _startingMoney,
                Science = _startingScience,
                Materials = _startingMaterials,
                Interceptor = CreateInterceptor(),
                Cruiser = CreateCruiser(),
                Dreadnaught = CreateDreadnaught(),
                Starbase = CreateStarbase()
            };
        }

        protected virtual ShipBlueprint CreateInterceptor()
        {
            return new ShipBlueprint
                       {
                           BaseInitiative = 2,
                           Cost = 3,
                           PartSpaces = 4,
                           Parts =
                               {
                                   PartFactory.IonCannon(),
                                   PartFactory.NuclearDrive(),
                                   PartFactory.NuclearSource()
                               }
                       };
        }

        protected virtual ShipBlueprint CreateCruiser()
        {
            return new ShipBlueprint
                       {
                           BaseInitiative = 1,
                           Cost = 5,
                           PartSpaces = 6,
                           Parts =
                               {
                                   PartFactory.Hull(),
                                   PartFactory.IonCannon(),
                                   PartFactory.NuclearSource(),
                                   PartFactory.ElectronComputer(),
                                   PartFactory.NuclearDrive()
                               }
                       };
        }

        protected virtual ShipBlueprint CreateDreadnaught()
        {
            return new ShipBlueprint
                       {
                           Cost = 8,
                           PartSpaces = 8,
                           Parts =
                               {
                                   PartFactory.Hull(),
                                   PartFactory.Hull(),
                                   PartFactory.IonCannon(),
                                   PartFactory.IonCannon(),
                                   PartFactory.NuclearSource(),
                                   PartFactory.ElectronComputer(),
                                   PartFactory.NuclearDrive()
                               }
                       };
        }

        protected virtual ShipBlueprint CreateStarbase()
        {
            return new ShipBlueprint
                       {
                           BaseInitiative = 4,
                           BasePower = 3,
                           Cost = 3,
                           PartSpaces = 5,
                           Parts =
                               {
                                   PartFactory.Hull(),
                                   PartFactory.Hull(),
                                   PartFactory.IonCannon(),
                                   PartFactory.ElectronComputer()
                               }
                       };
        }
    }
}