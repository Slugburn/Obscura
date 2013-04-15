using System.Linq;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Factions
{
    public abstract class FactionType : IFactionType
    {
        private readonly int _startingMoney;
        private readonly int _startingScience;
        private readonly int _startingMaterials;

        protected FactionType(FactionColor color, string name, int homeSectorId, int startingMoney, int startingScience, int startingMaterials)
        {
            Name = name;
            Color = color;
            HomeSectorId = homeSectorId;
            _startingMoney = startingMoney;
            _startingScience = startingScience;
            _startingMaterials = startingMaterials;
        }

        public FactionColor Color { get; private set; }

        public string Name { get; private set; }

        public int HomeSectorId { get; private set; }

        public virtual void Setup(PlayerFaction faction)
        {
            faction.Money = _startingMoney;
            faction.Science = _startingScience;
            faction.Material = _startingMaterials;
            faction.Interceptor = CreateInterceptor();
            faction.Cruiser = CreateCruiser();
            faction.Dreadnought = CreateDreadnought();
            faction.Starbase = CreateStarbase();
            faction.ColonyShips = 3;
            faction.MaxColonyShips = 3;
            faction.Influence = 13;
        }

        protected virtual ShipBlueprint CreateInterceptor()
        {
            return new ShipBlueprint
                       {
                           ShipType = ShipType.Interceptor,
                           Name = "Interceptor",
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
                           ShipType = ShipType.Cruiser,
                           Name = "Cruiser",
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

        protected virtual ShipBlueprint CreateDreadnought()
        {
            return new ShipBlueprint
                       {
                           ShipType = ShipType.Dreadnought,
                           Name = "Dreadnought",
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
            return new StarbaseBlueprint
                       {
                           Name = "Starbase",
                           BaseInitiative = 4,
                           BaseEnergy = 3,
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