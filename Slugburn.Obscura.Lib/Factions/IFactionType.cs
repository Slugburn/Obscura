namespace Slugburn.Obscura.Lib.Factions
{
    public interface IFactionType
    {
        void Setup(PlayerFaction faction);
        FactionColor Color { get; }
        string Name { get; }
        int HomeSectorId { get; }
    }
}
