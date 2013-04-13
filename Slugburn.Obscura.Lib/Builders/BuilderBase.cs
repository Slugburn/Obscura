using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Builders
{
    public abstract class BuilderBase : IBuilder
    {
        private readonly string _name;

        protected BuilderBase(string name)
        {
            _name = name;
        }

        public string Name { get { return _name; } }
        public abstract bool IsBuildAvailable(Faction faction);
        public abstract IBuildable Create(Faction faction);
        public abstract int CostFor(Faction faction);
        public abstract double CombatEfficiencyFor(Faction faction);
        
        
        public virtual bool OnePerSector
        {
            get { return false; }
        }

        public virtual bool IsValidPlacementLocation(Sector sector)
        {
            return true;
        }

    }
}