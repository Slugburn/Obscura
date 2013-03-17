using System.Collections.Generic;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib
{
    public abstract class Faction : IFaction
    {
        private readonly int _startingMoney;
        private readonly int _startingScience;
        private readonly int _startingMaterials;

        protected Faction(PlayerColor color, string name, int homeSectorId, int startingMoney, int startingScience, int startingMaterials)
        {
            Name = name;
            Color = color;
            HomeSectorId = homeSectorId;
            _startingMoney = startingMoney;
            _startingScience = startingScience;
            _startingMaterials = startingMaterials;
        }

        public PlayerColor Color { get; private set; }

        public string Name { get; private set; }

        public int HomeSectorId { get; private set; }

        public virtual void Setup(Player player)
        {
            player.Money = _startingMoney;
            player.Science = _startingScience;
            player.Materials = _startingMaterials;
            player.Interceptor = CreateInterceptor();
            player.Cruiser = CreateCruiser();
            player.Dreadnaught = CreateDreadnaught();
            player.Starbase = CreateStarbase();
            player.ColonyShips = 3;
            player.Influence = 13;
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