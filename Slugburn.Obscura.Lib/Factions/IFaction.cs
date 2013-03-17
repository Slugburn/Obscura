namespace Slugburn.Obscura.Lib.Factions
{
    public interface IFaction
    {
        void Setup(Player player);
        PlayerColor Color { get; }
        string Name { get; }
        int HomeSectorId { get; }
    }
}
