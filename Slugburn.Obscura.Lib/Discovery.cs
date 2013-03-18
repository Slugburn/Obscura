namespace Slugburn.Obscura.Lib
{
    public class Discovery
    {
        public string Name { get; set; }

        public Discovery(string name)
        {
            Name = name;
        }

        public void Use(Player player)
        {
            throw new System.NotImplementedException();
        }
    }
}