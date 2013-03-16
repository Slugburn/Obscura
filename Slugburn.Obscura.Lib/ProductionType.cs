namespace Slugburn.Obscura.Lib
{
    public enum ProductionType
    {
        Money = 1,
        Science = 2,
        Material = 4,
        Orbital = Money | Science,
        Any = Money | Science | Material
    }
}