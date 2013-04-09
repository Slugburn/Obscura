using System.Collections.Generic;
using System.Linq;

namespace Slugburn.Obscura.Lib.Technology
{
    public class TechCatalog
    {
        public static IList<Tech> GetTiles()
        {
            var techs = new[]
                            {
                                Tech.AdvancedEconomy, Tech.AdvancedLabs, Tech.AdvancedMining, Tech.AdvancedRobotics, Tech.AntimatterCannon,
                                Tech.ArtifactKey, Tech.FusionDrive, Tech.FusionSource, Tech.GaussShield, Tech.GluonComputer, Tech.ImprovedHull,
                                Tech.Monolith, Tech.Nanorobots, Tech.NeutronBombs, Tech.Orbital, Tech.PhaseShield, Tech.PlasmaCannon,
                                Tech.PlasmaMissile, Tech.PositronComputer, Tech.QuantumGrid, Tech.Starbase, Tech.TachyonDrive, Tech.TachyonSource,
                                Tech.WormholeGenerator
                            };
            return CreateTiles(techs);
        }

        private static IList<Tech> CreateTiles(IEnumerable<Tech> techs)
        {
            return techs.SelectMany(tech => Enumerable.Range(1, GetTileCount(tech)).Select(x=>tech)).ToList();
        }

        private static int GetTileCount(Tech tech)
        {
            if (tech.Cost <= 6) return 5;
            if (tech.Cost >= 12) return 3;
            // else
            return 4;
        }
    }
}
