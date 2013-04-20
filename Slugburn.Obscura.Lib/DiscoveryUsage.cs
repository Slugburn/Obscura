using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib
{
    public class DiscoveryUsage
    {
        public DiscoveryUsage(Faction faction, Sector discoveredIn)
        {
            Faction = faction;
            DiscoveredIn = discoveredIn;
        }

        public Faction Faction { get; private set; }

        public Sector DiscoveredIn { get; private set; }
    }
}