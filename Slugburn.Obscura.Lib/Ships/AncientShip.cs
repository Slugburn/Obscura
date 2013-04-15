using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Ships
{
    public class AncientShip : Ship
    {
        public AncientShip(AncientFaction faction)
        {
            Faction = faction;
        }

        private static readonly ShipProfile _profile = new ShipProfile
            {
                Cannons = new[] {1, 1},
                Accuracy = 1,
                Structure = 1,
                Initiative = 2
            };

        public override ShipProfile Profile
        {
            get { return _profile; }
        }

        public override ShipType ShipType
        {
            get { return ShipType.AncientShip; }
        }

        public override string ToString()
        {
            return "Ancient Ship";
        }
    }
}
