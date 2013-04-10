using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Ships
{
    public abstract class Ship
    {
        public Sector Sector { get; set; }

        public Faction Faction { get; set; }

        public abstract ShipProfile Profile { get; }
    }
}