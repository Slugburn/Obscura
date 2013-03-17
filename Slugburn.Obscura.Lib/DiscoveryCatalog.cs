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
                           Money().Repeat(3),
                           Science().Repeat(3),
                           Materials().Repeat(3),
                           AncientTechnology().Repeat(3),
                           AncientCruiser().Repeat(3),
                           new[]
                               {
                                   AxiomComputer(),
                                   HypergridSource(),
                                   ShardHull(),
                                   IonTurret(),
                                   ConformalDrive(),
                                   FluxShield()
                               }
                       }
                .SelectMany(x => x);
        }

        private static Discovery Money()
        {
            return new Discovery("+8 Money");
        }

        private static Discovery Science()
        {
            return new Discovery("+5 Science");
        }

        private static Discovery Materials()
        {
            return new Discovery("+6 Materials");
        }

        private static Discovery AncientTechnology()
        {
            // choose lowest cost unknown tech
            return new Discovery("Ancient Technology");
        }

        private static Discovery AncientCruiser()
        {
            // get free cruiser
            return new Discovery("Ancient Cruiser");
        }

        private static Discovery AxiomComputer()
        {
            // +3 accuracy
            return new Discovery("Axiom Computer");
        }

        private static Discovery HypergridSource()
        {
            // +11 energy
            return new Discovery("Hypergrid Source");
        }

        private static Discovery ShardHull()
        {
            // 3 structure
            return new Discovery("Shard Hull");
        }

        private static Discovery IonTurret()
        {
            // 2 x 1 damage, -1 energy
            return new Discovery("Ion Turret");
        }

        private static Discovery ConformalDrive()
        {
            // 4 move, 2 init, -2 energy
            return new Discovery("Conformal Drive");
        }

        private static Discovery FluxShield()
        {
            // -3 deflection, -2 energy
            return new Discovery("Flux Shield");
        }


    }
}