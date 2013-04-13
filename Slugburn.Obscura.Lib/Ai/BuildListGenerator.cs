using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Ai.Actions;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Ai
{
    public class BuildListGenerator
    {
        private readonly IEnumerable<IBuilder> _builders;

        public BuildListGenerator(IEnumerable<IBuilder> builders)
        {
            _builders = builders;
        }

        public IList<BuildLocation> Generate(Faction faction, IEnumerable<Sector> validLocations, Func<IBuilder, decimal> builderRating )
        {
            var builders = _builders.Where(b => b.IsBuildAvailable(faction));
            var builderStates = builders.Select(b => CreateBuilderState(faction, validLocations, b));
            var buildLists = Generate(faction.BuildCount, faction.Material, builderStates.ToArray());
            var bestList = buildLists.OrderByDescending(x => x.Sum(y => builderRating(y.Builder))).FirstOrDefault();
            return bestList;
        }

        private List<List<BuildLocation>> Generate(int buildCount, int material, IEnumerable<BuilderState> builderStates)
        {
            var lists = new List<List<BuildLocation>>();
            var validStates = builderStates.Where(s => s.Cost <= material && s.NumberAvailable > 0 && s.PlacementLocations.Any()).ToArray();
            foreach (var state in validStates)
            {
                var buildLocation = new BuildLocation {Builder = state.Builder, Location = state.PlacementLocations.PickRandom()};
                var list = new List<BuildLocation> {buildLocation};
                lists.Add(list);
                if (buildCount>1)
                {
                    var updatedStates = validStates.ToList();
                    updatedStates.Remove(state);
                    updatedStates.Add(new BuilderState
                                          {
                                              Builder = state.Builder,
                                              Cost = state.Cost,
                                              NumberAvailable = state.NumberAvailable - 1,
                                              PlacementLocations = state.Builder is IOnePerSectorBuilder
                                                                       ? state.PlacementLocations.Except(new[] {buildLocation.Location})
                                                                       : state.PlacementLocations
                                          });
                    var additional = Generate(buildCount - 1, material - state.Cost, updatedStates);
                    foreach (var a in additional)
                        a.Add(buildLocation);
                    lists.AddRange(additional);
                }
            }
            return lists;
        }

        private static BuilderState CreateBuilderState(Faction faction, IEnumerable<Sector> validLocations, IBuilder b)
        {
            return new BuilderState
                       {
                           Builder=b, 
                           Cost = b.CostFor(faction), 
                           PlacementLocations = validLocations.Where(b.IsValidPlacementLocation),
                           NumberAvailable = b is IShipBuilder ? ((IShipBuilder)b).MaximumBuildableFor(faction) : int.MaxValue
                       };
        }

        private class BuilderState
        {
            public IBuilder Builder { get; set; }

            public int Cost { get; set; }

            public IEnumerable<Sector> PlacementLocations { get; set; }

            public int NumberAvailable { get; set; }
        }
    }

}
