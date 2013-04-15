using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib
{
    public class Discovery
    {
        public string Name { get; set; }

        public Discovery(string name)
        {
            Name = name;
        }

        public void Use(PlayerFaction faction)
        {
            throw new System.NotImplementedException();
        }
    }
}