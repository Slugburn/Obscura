using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Actions
{
    public class BuildAction : IAction
    {
        private readonly ILog _log;

        private readonly IEnumerable<IBuilder> _builders;

        public string Name { get { return "Build"; } }

        public BuildAction(IEnumerable<IBuilder> builders, ILog log)
        {
            _builders = builders;
            _log = log;
        }

        public void Do(Faction faction)
        {
            var buildsCompleted = 0;
            while (buildsCompleted < faction.BuildCount)
            {
                var validBuilders = _builders.Where(b => b.IsBuildAvailable(faction)).ToList();
                if (validBuilders.Count == 0)
                    break;
                var builder = faction.Player.ChooseBuilder(validBuilders);
                if (builder == null)
                    break;
                var built = builder.Create(faction);
                var validPlacementLocations = faction.Sectors.Where(builder.IsValidPlacementLocation).ToList();
                var placementLocation = faction.Player.ChoosePlacementLocation(built, validPlacementLocations);
                built.Place(placementLocation);
                _log.Log("{0} expends {1} Material to build {2} in {3} ({4})", faction.Name, builder.CostFor(faction), built.Name, placementLocation.Name, placementLocation.Id);
                buildsCompleted++;
            }
        }

        public bool IsValid(Faction faction)
        {
            return !faction.Passed && faction.Influence > 0 && _builders.Any(builder => builder.IsBuildAvailable(faction));
        }

    }
}