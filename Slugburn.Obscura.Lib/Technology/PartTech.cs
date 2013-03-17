using System;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Technology
{
    public class PartTech : Tech
    {
        private readonly Func<ShipPart> _factory;

        public PartTech(string name, int cost, int minCost, TechCategory category, Func<ShipPart> factory)
            : base(name,cost,minCost, category)
        {
            _factory = factory;
        }
    }
}