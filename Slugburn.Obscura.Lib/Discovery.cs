using System;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib
{
    public class Discovery
    {
        private readonly Action<PlayerFaction> _onUse;
        public string Name { get; set; }

        public Discovery(string name, Action<PlayerFaction> onUse)
        {
            _onUse = onUse;
            Name = name;
        }

        public void Use(PlayerFaction faction)
        {
            _onUse(faction);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}