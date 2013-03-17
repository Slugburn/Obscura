using System.Collections.Generic;

namespace Slugburn.Obscura.Lib.Factions
{
    public static class FactionCatalog
    {
        public static IList<IFaction> GetFactions()
        {
            return new IFaction[]
                       {
                           new HumanFaction(PlayerColor.Red, "Terran Directorate", 221),
                           new HumanFaction(PlayerColor.Blue, "Terran Federation", 223),
                           new HumanFaction(PlayerColor.Green, "Terran Union", 225),
                           new HumanFaction(PlayerColor.Yellow, "Terran Republic", 227),
                           new HumanFaction(PlayerColor.White, "Terran Conglomerate", 229),
                           new HumanFaction(PlayerColor.Black, "Terran Alliance", 231),
                       };
        }
    }
}
