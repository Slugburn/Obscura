using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Actions
{
    public class BuildAction : IAction
    {
        private readonly IEnumerable<IBuilder> _builders;

        public BuildAction(IEnumerable<IBuilder> builders)
        {
            _builders = builders;
        }

        public void Do(Faction faction)
        {
            var buildsCompleted = 0;
            while (buildsCompleted < faction.BuildCount)
            {
                var validBuilders = _builders.Where(b => b.IsBuildAvailable(faction));
                var builder = faction.Player.ChooseBuilder(validBuilders);
                if (builder == null)
                    break;
                var built = builder.Create(faction);
                var validPlacementLocations = faction.Sectors.Where(builder.IsValidPlacementLocation).ToList();
                var placementLocation = validPlacementLocations.Count == 1
                                            ? validPlacementLocations[0]
                                            : faction.Player.ChoosePlacementLocation(built, validPlacementLocations);
                built.Place(placementLocation);
                buildsCompleted++;
            }
        }

        public bool IsValid(Faction faction)
        {
            return _builders.Any(builder => builder.IsBuildAvailable(faction));
        }
    }
}