namespace Slugburn.Obscura.Lib.Factions
{
    public class HumanFaction : Faction
    {
        public HumanFaction(PlayerColor color, string name, int homeSectorId) : base(color, name, homeSectorId, 2,3,3)
        {
        }
    }
}