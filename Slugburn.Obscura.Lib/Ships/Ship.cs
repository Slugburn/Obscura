using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Ships
{
    public abstract class Ship
    {
        public Sector Sector { get; set; }

        public IShipOwner Faction { get; set; }

        public abstract ShipProfile Profile { get; }

        public decimal Rating
        {
            get { return Profile.Rating; }
        }

        public int Damage { get; set; }

        public int RemainingStructure
        {
            get { return Profile.Structure - Damage; }
        }

        public abstract ShipType ShipType { get; }

        public int Accuracy
        {
            get { return Profile.Accuracy; }
        }

        public int[] Cannons
        {
            get { return Profile.Cannons; }
        }
    }
}