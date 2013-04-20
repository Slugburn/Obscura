using System;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Technology
{
    public class EffectTech : Tech
    {
        private readonly Action<Faction> _effect;

        public EffectTech(string name, int cost, int minCost, TechCategory category, Action<Faction> effect) : base(name, cost, minCost, category)
        {
            _effect = effect;
        }

        public void OnResearch(Faction faction)
        {
            _effect(faction);
        }
    }
}
