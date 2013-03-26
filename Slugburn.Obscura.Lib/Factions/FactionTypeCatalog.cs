using System.Collections.Generic;

namespace Slugburn.Obscura.Lib.Factions
{
    public static class FactionTypeCatalog
    {
        public static IList<IFactionType> GetFactionTypes()
        {
            return new IFactionType[]
                       {
                           new HumanFactionType(FactionColor.Red, "Terran Directorate", 221),
                           new HumanFactionType(FactionColor.Blue, "Terran Federation", 223),
                           new HumanFactionType(FactionColor.Green, "Terran Union", 225),
                           new HumanFactionType(FactionColor.Yellow, "Terran Republic", 227),
                           new HumanFactionType(FactionColor.White, "Terran Conglomerate", 229),
                           new HumanFactionType(FactionColor.Black, "Terran Alliance", 231),
                       };
        }
    }
}
