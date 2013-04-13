using System.Linq;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Builders
{
    public abstract class BuilderBase : IBuilder
    {
        protected bool Equals(BuilderBase other)
        {
            return string.Equals(_name, other._name);
        }

        public override int GetHashCode()
        {
            return (_name != null ? _name.GetHashCode() : 0);
        }

        private readonly string _name;

        protected BuilderBase(string name)
        {
            _name = name;
        }

        public string Name { get { return _name; } }
        public abstract IBuildable Create(Faction faction);
        public abstract int CostFor(Faction faction);
        public abstract decimal CombatEfficiencyFor(Faction faction);
        

        public bool IsBuildAvailable(Faction faction)
        {
            if (RequiredTech != null && !faction.HasTechnology(RequiredTech))
                return false;
            if (faction.Material < CostFor(faction))
                return false;
            var shipBuilder = this as ShipBuilder;
            if (shipBuilder != null && shipBuilder.MaximumBuildableFor(faction) == 0)
                return false;
            var onePerSectorBuilder = this as IOnePerSectorBuilder;
            if (onePerSectorBuilder != null && faction.Sectors.All(onePerSectorBuilder.HasBeenBuilt))
                return false;
            return true;
        }

        public virtual bool OnePerSector
        {
            get { return false; }
        }

        public abstract Tech RequiredTech { get; }

        public virtual bool IsValidPlacementLocation(Sector sector)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals((BuilderBase)obj);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}