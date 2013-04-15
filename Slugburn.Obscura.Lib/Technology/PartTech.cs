using System;
using Slugburn.Obscura.Lib.Ships;

namespace Slugburn.Obscura.Lib.Technology
{
    public class PartTech : Tech
    {
        public PartType PartType { get; set; }
        private readonly Func<ShipPart> _factory;

        public PartTech(string name, int cost, int minCost, TechCategory category, PartType partType, Func<ShipPart> factory)
            : base(name,cost,minCost, category)
        {
            PartType = partType;
            _factory = factory;
        }

        public ShipPart CreatePart()
        {
            var part = _factory();
            part.Name = Name;
            return part;
        }
    }
}