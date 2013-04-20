namespace Slugburn.Obscura.Lib.Factions
{
    public interface IFactionType
    {
        void Setup(Faction faction);
        FactionColor Color { get; }
        string Name { get; }
        int HomeSectorId { get; }
    }
}
