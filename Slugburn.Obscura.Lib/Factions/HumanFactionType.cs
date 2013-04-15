using Slugburn.Obscura.Lib.Technology;

namespace Slugburn.Obscura.Lib.Factions
{
    public abstract class HumanFactionType : FactionType
    {
        protected HumanFactionType(FactionColor color, string name, int homeSectorId) : base(color, name, homeSectorId, 2,3,3)
        {
        }

        public override void Setup(PlayerFaction faction)
        {
            base.Setup(faction);
            faction.Technologies.Add(Tech.Starbase);
        }

        public class TerranDirectorate : HumanFactionType
        {
            public TerranDirectorate()
                : base(FactionColor.Red, "Terran Directorate", 221)
            {
            }
        }

        public class TerranFederation : HumanFactionType
        {
            public TerranFederation()
                : base(FactionColor.Blue, "Terran Federation", 223)
            {
            }
        }

        public class TerranUnion : HumanFactionType
        {
            public TerranUnion()
                : base(FactionColor.Green, "Terran Union", 225)
            {
            }
        }

        public class TerranRepublic : HumanFactionType
        {
            public TerranRepublic()
                : base(FactionColor.Yellow, "Terran Republic", 227)
            {
            }
        }

        public class TerranConglomerate : HumanFactionType
        {
            public TerranConglomerate()
                : base(FactionColor.White, "Terran Conglomerate", 229)
            {
            }
        }

        public class TerranAlliance : HumanFactionType
        {
            public TerranAlliance()
                : base(FactionColor.Black, "Terran Alliance", 231)
            {
            }
        }
    }
}