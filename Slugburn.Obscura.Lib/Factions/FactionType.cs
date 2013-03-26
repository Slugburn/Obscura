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

        public virtual void Setup(Faction faction)
        {
            faction.Money = _startingMoney;
            faction.Science = _startingScience;
            faction.Materials = _startingMaterials;
            faction.Interceptor = CreateInterceptor();
            faction.Cruiser = CreateCruiser();
            faction.Dreadnought = CreateDreadnaught();
            faction.Starbase = CreateStarbase();
            faction.ColonyShips = 3;
            faction.Influence = 13;
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