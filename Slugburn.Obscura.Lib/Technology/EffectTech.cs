using System;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Technology
{
    public class EffectTech : Tech
    {
        private readonly Action<PlayerFaction> _effect;

        public EffectTech(string name, int cost, int minCost, TechCategory category, Action<PlayerFaction> effect) : base(name, cost, minCost, category)
        {
            _effect = effect;
        }

        public void OnResearch(PlayerFaction faction)
        {
            _effect(faction);
        }
    }
}
