using System;
using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Actions;
using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Extensions;
using Slugburn.Obscura.Lib.Factions;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Ai.Generators
{
    public class BuildListGenerator
    {
        private readonly IEnumerable<IBuilder> _builders;

        public BuildListGenerator(IEnumerable<IBuilder> builders)
        {
            _builders = builders;
        }

        public IList<BuildLocation> Generate(IAiPlayer player, IEnumerable<Sector> validLocations, Func<Faction, IBuilder, decimal> builderRating )
        {
            var faction = player.Faction;
            var builders = _builders.Where(b => b.IsBuildAvailable(faction)).ToList();

            var builderStates = builders.Select(b => CreateBuilderState(faction, validLocations, b)).ToArray();
            var buildLists = Generate(faction.BuildCount, faction.Material, builderStates.ToArray());
            var rated = buildLists.Select(x => new {list = x, rating = x.Sum(y => builderRating(faction, y.Builder))}).ToArray();
            var best = rated.Where(x=>x.rating> 0).OrderByDescending(x => x.rating).FirstOrDefault();
            return best != null ? best.list : null;
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
                if (buildCount == 0) 
                    continue;
                var updatedStates = validStates.ToList();
                updatedStates.Remove(state);
                updatedStates.Add(new BuilderState
                    {
                        Builder = state.Builder,
                        Cost = state.Cost,
                        NumberAvailable = state.NumberAvailable - 1,
                        PlacementLocations = state.Builder.OnePerSector
                                                 ? state.PlacementLocations.Except(new[] {buildLocation.Location})
                                                 : state.PlacementLocations
                    });
                var additional = Generate(buildCount - 1, material - state.Cost, updatedStates);
                foreach (var a in additional)
                    a.Add(buildLocation);
                lists.AddRange(additional);
            }
            return lists;
        }

        private static BuilderState CreateBuilderState(Faction faction, IEnumerable<Sector> validLocations, IBuilder b)
        {
            var locations = validLocations.ToArray();
            if (locations.Any(x=>x==null))
                throw new ArgumentException("Null locations are not valid");
            return new BuilderState
                       {
                           Builder=b, 
                           Cost = b.CostFor(faction), 
                           PlacementLocations = locations.Where(b.IsValidPlacementLocation),
                           NumberAvailable = b is IShipBuilder ? ((IShipBuilder)b).MaximumBuildableFor(faction) : Int32.MaxValue
                       };
        }

        private class BuilderState
        {
            public IBuilder Builder { get; set; }

            public int Cost { get; set; }

            public IEnumerable<Sector> PlacementLocations { get; set; }

            public int NumberAvailable { get; set; }
        }

        public static Func<Faction, IBuilder, decimal> RateCombatEfficiency
        {
            get { return (f, b) =>
                             {
                                 var rating = b.CombatEfficiencyFor(f);
                                 return rating;
                             }; }
        }

        public static Func<Faction, IBuilder, decimal> RateAttackEfficency
        {
            get
            {
                return (f, b) =>
                           {
                               var sb = b as IShipBuilder;
                               var rating = sb != null ? (sb.CanMove ? RateCombatEfficiency(f, b) : 0) : 0;
                               return rating;
                           };
            }
        }

        public static Func<Faction, IBuilder, decimal> RateEconomicEfficiency
        {
            get
            {
                return (f, b) =>
                           {
                               if (b.CombatEfficiencyFor(f) == 0)
                                   return b.CostFor(f)*1000;
                               return 0;
                           };
            }
        }

        public decimal RateStagingPoint(IAiPlayer player, Func<Faction, IBuilder, decimal> rateEfficiency)
        {
            if (player.GetAction<BuildAction>() == null) 
                return 0;
            player.BuildList = Generate(player, new[] {player.StagingPoint}, rateEfficiency);
            if (player.BuildList == null)
                return 0;
            return player.BuildList.Sum(x => x.Rating)*((decimal) player.BuildList.Count/player.Faction.BuildCount);
        }

        public decimal RateAllSectors(IAiPlayer player, Func<Faction, IBuilder, decimal> rateEfficiency)
        {
            if (player.GetAction<BuildAction>() == null) 
                return 0;
            player.BuildList = Generate(player, player.Faction.Sectors, rateEfficiency);
            if (player.BuildList == null)
                return 0;
            return player.BuildList.Sum(x => x.Rating)*((decimal) player.BuildList.Count/player.Faction.BuildCount);
        }
    }

}
