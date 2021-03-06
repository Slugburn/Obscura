﻿using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Messages;

namespace Slugburn.Obscura.Lib.Actions
{
    public class BuildAction : IAction
    {
        private readonly ILog _log;

        private readonly IEnumerable<IBuilder> _builders;

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
                var sector = faction.Player.ChoosePlacementLocation(built, validPlacementLocations);
                if (validPlacementLocations.All(x => x != sector))
                    throw new InvalidOperationException(string.Format("Placing {0} at {1} is not valid.", built, sector));
                built.Place(sector);
                _log.Log("\t{0} built in {1} ({2} Material)", built, sector, builder.CostFor(faction));
                faction.Game.SendMessage(new SectorUpdated(sector));
                buildsCompleted++;
            }
        }

        public bool IsValid(Faction faction)
        {
            return faction.Influence > 0 && _builders.Any(builder => builder.IsBuildAvailable(faction));
        }

        public override string ToString()
        {
            return "Build";
        }

    }
}