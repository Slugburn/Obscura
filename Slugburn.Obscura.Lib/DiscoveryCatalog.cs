using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Extensions;

namespace Slugburn.Obscura.Lib
{
    public class DiscoveryCatalog
    {
        public static IEnumerable<Discovery> GetTiles()
        {
            return new[]
                       {
                           Discovery.Money.Repeat(3),
                           Discovery.Science.Repeat(3),
                           Discovery.Materials.Repeat(3),
                           Discovery.AncientTechnology.Repeat(3),
                           Discovery.AncientCruiser.Repeat(3),
                           new[]
                               {
                                   Discovery.AxiomComputer,
                                   Discovery.HypergridSource,
                                   Discovery.ShardHull,
                                   Discovery.IonTurret,
                                   Discovery.ConformalDrive,
                                   Discovery.FluxShield
                               }
                       }
                .SelectMany(x => x);
        }
    }
}