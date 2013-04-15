using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Ships
{
    public abstract class Ship
    {
        public Sector Sector { get; set; }

        public IFaction Faction { get; set; }

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
    }

    public enum ShipType
    {
        Interceptor,
        Cruiser,
        Dreadnought,
        Starbase,
        AncientShip,
        GCDS
    }
}