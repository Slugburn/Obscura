using System;
using System.Linq;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Actions
{
    public class InfluenceAction : IAction
    {
        public void Do(Faction faction)
        {
            // recover two colony ships
            faction.ColonyShips = Math.Min(faction.ColonyShips+2, faction.MaxColonyShips);

            for (var i = 0; i < 2; i++)
            {
                var influenceDirection = faction.Player.ChooseInfluenceDirection();
                switch (influenceDirection)
                {
                    case InfluenceDirection.Place:
                        var validLocations = faction.GetInfluencePlacementLocations();
                        if (!validLocations.Any())
                            return;
                        var location = faction.Player.ChooseInfluencePlacementLocation(validLocations);
                        if (validLocations.All(x => location != x))
                            throw new InvalidOperationException(string.Format("Unable to influence {0}", location));
                        faction.ClaimSector(location);
                        break;
                    case InfluenceDirection.Remove:
                        throw new NotImplementedException();
                        break;
                    default:
                        return;
                }
            }
        }

        public bool IsValid(Faction faction)
        {
            return true;
        }

        public override string ToString()
        {
            return "Influence";
        }
    }
}
