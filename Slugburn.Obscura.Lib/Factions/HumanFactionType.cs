namespace Slugburn.Obscura.Lib.Factions
{
    public class HumanFactionType : FactionType
    {
        public HumanFactionType(FactionColor color, string name, int homeSectorId) : base(color, name, homeSectorId, 2,3,3)
        {
        }
    }
}