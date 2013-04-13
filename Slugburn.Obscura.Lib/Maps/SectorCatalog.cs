using System.Collections.Generic;
using System.Linq;
using Slugburn.Obscura.Lib.Extensions;

namespace Slugburn.Obscura.Lib.Maps
{
    class SectorCatalog
    {
        public static Dictionary<int, Sector> GetSectors()
        {
            return new[]
                       {
                           Sector(1, "Galactic Center").Vp(4).Artifact().Discovery()
                               .Population(Science, AdvScience, Material, AdvMaterial, Any, Any).Wormholes(12, 2, 4, 6, 8, 10),
                           Sector(101, "Castor").Vp(2).Ancient().Population(Money, Material, AdvMaterial).Wormholes(2, 4, 6, 8, 10),
                           Sector(102, "Pollux").Vp(3).Artifact().Population(Science, AdvScience).Wormholes(12, 2, 6, 8),
                           Sector(103, "Beta Leonis").Vp(2).Population(AdvMoney, Any).Wormholes(12, 2, 4, 8, 10),
                           Sector(104, "Arcturus").Vp(2).Ancient(2).Population(Money, AdvMoney, Science, AdvScience).Wormholes(12, 2, 6, 8),
                           Sector(105, "Zeta Hurculis").Vp(3).Artifact().Ancient().Population(Money, Science, AdvMaterial).Wormholes(12, 2, 6, 8, 10),
                           Sector(106, "Capella").Vp(2).Population(Science, Material).Wormholes(12, 2, 4, 6),
                           Sector(107, "Aldebaran").Vp(2).Population(Money, AdvScience, AdvMaterial).Wormholes(12, 2, 4, 6, 10),
                           Sector(108, "Mu Cassiopaeiae").Vp(2).Ancient().Population(AdvMoney, Science, Any).Wormholes(12, 2, 6, 8),
                           Sector(201, "Alpha Centauri").Vp(1).Population(Money, Material).Wormholes(2, 6, 10),
                           Sector(202, "Fomalhaut").Vp(1).Population(Science, AdvScience).Wormholes(2, 6, 10),
                           Sector(203, "Chi Draconis").Vp(1).Ancient(2).Population(Money, Science, Material).Wormholes(12, 2, 6, 10),
                           Sector(204, "Vega").Vp(2).Artifact().Ancient().Population(AdvMoney, AdvMaterial, Any).Wormholes(12, 2, 6, 10),
                           Sector(205, "Mu Herculis").Vp(1).Population(Money, AdvMoney, Science).Wormholes(4, 6, 8),
                           Sector(206, "Epsilon Indi").Vp(1).Discovery().Population(Material).Wormholes(2, 4, 6, 10),
                           Sector(207, "Zeta Reticuli").Vp(1).Discovery().Population().Wormholes(12, 2, 6),
                           Sector(208, "Iota Persei").Vp(1).Discovery().Population().Wormholes(12, 4, 6, 10),
                           Sector(209, "Delta Eridani").Vp(1).Population(AdvMoney, Science).Wormholes(12, 2, 6, 10),
                           Sector(210, "Psi Capricorni").Vp(1).Population(Money, Material).Wormholes(12, 6, 10),
                           Sector(211, "Beta Aquilae").Vp(2).Artifact().Ancient(2).Population(Money, AdvMaterial, Any).Wormholes(12, 2, 4, 6),
                           Sector(221, "Procyon").Vp(3).Artifact().Population(Money, AdvMoney, Science, AdvScience, Material).Wormholes(12, 2, 6, 8),
                           Sector(222, "Epsilon Eridani").Vp(3).Artifact().Population(Money, AdvMoney, Science, AdvScience).Wormholes(12, 2, 6, 8),
                           Sector(223, "Altair").Vp(3).Artifact().Population(Money, AdvMoney, Science, AdvScience, Material).Wormholes(12, 2, 6, 8),
                           Sector(224, "Beta Hydri").Vp(3).Artifact().Population(Money, Science, AdvScience, AdvMaterial).Wormholes(12, 2, 6, 8),
                           Sector(225, "Eta Cassiopeiae").Vp(3).Artifact().Population(Money, AdvMoney, Science, AdvScience, Material).Wormholes(12, 2, 6, 8),
                           Sector(226, "51 Cygni").Vp(3).Artifact().Population(Money, Science, Material).Wormholes(12, 2, 6, 8),
                           Sector(227, "Sirius").Vp(3).Artifact().Population(Money, AdvMoney, Science, AdvScience, Material).Wormholes(12, 2, 6, 8),
                           Sector(228, "Sigma Draconis").Vp(3).Artifact().Population(Money, Science, AdvMaterial).Wormholes(12, 2, 6, 8),
                           Sector(229, "Tau Ceti").Vp(3).Artifact().Population(Money, AdvMoney, Science, AdvScience, Material).Wormholes(12, 2, 6, 8),
                           Sector(230, "Lambda Aurigae").Vp(3).Artifact().Population(Money, AdvMoney, Science, AdvMaterial).Wormholes(12, 2, 6, 8),
                           Sector(231, "Delta Pavonis").Vp(3).Artifact().Population(Money, AdvMoney, Science, AdvScience, Material).Wormholes(12, 2, 6, 8),
                           Sector(232, "Rigel").Vp(3).Artifact().Population(AdvMoney, Science, Material, AdvMaterial).Wormholes(12, 2, 6, 8),
                           Sector(301, "Zeta Draconis").Vp(2).Artifact().Ancient(2).Population(Money, Science, AdvMaterial).Wormholes(12, 4, 6),
                           Sector(302, "Gamma Serpentis").Vp(2).Artifact().Ancient().Population(AdvMoney, Material).Wormholes(12, 6, 8),
                           Sector(303, "Eta Cephei").Vp(2).Artifact().Ancient().Population(Any).Wormholes(6, 10),
                           Sector(304, "Theta Pegasi").Vp(1).Population(AdvMoney, Material).Wormholes(12, 6),
                           Sector(305, "Lambda Serpentis").Vp(1).Ancient().Population(Science, Material).Wormholes(12, 2, 6),
                           Sector(306, "Beta Centauri").Vp(1).Population(Money, Material).Wormholes(2, 6),
                           Sector(307, "Sigma Sagittarii").Vp(1).Population(Money, AdvScience).Wormholes(12, 4, 6),
                           Sector(308, "Kappa Scorpii").Vp(1).Population(Science, AdvMaterial).Wormholes(4, 6, 10),
                           Sector(309, "Phi Piscium").Vp(1).Population(Money, AdvScience).Wormholes(12, 6, 10),
                           Sector(310, "Nu Phoenicis").Vp(1).Population(Science, Material).Wormholes(12, 6),
                           Sector(311, "Canopus").Vp(1).Discovery().Population(Material).Wormholes(12, 4, 6),
                           Sector(312, "Antares").Vp(1).Discovery().Population(Material).Wormholes(12, 2, 6),
                           Sector(313, "Alpha Ursae Minoris").Vp(1).Discovery().Population(Any).Wormholes(12, 6),
                           Sector(314, "Spica").Vp(1).Discovery().Population(Any).Wormholes(4, 6, 8),
                           Sector(315, "Epsilon Aurigae").Vp(1).Discovery().Population().Wormholes(12, 6, 10),
                           Sector(316, "Iota Carinae").Vp(1).Discovery().Population().Wormholes(12, 2, 6),
                           Sector(317, "Beta Crucis").Vp(1).Population(Money, AdvMoney).Wormholes(6, 8),
                           Sector(318, "Gamma Veldrum").Vp(1).Population(AdvMaterial, Any).Wormholes(4, 6)
                       }
                .ToDictionary(x => x.Id);
        }

        protected static PopulationSquare Science
        {
            get { return new PopulationSquare(ProductionType.Science, false); }
        }

        protected static PopulationSquare AdvScience
        {
            get { return new PopulationSquare(ProductionType.Science, true); }
        }

        protected static PopulationSquare Money
        {
            get { return new PopulationSquare(ProductionType.Money, false); }
        }

        protected static PopulationSquare AdvMoney
        {
            get { return new PopulationSquare(ProductionType.Money, true); }
        }

        protected static PopulationSquare Material
        {
            get {return new PopulationSquare(ProductionType.Material, false);}
        }

        protected static PopulationSquare AdvMaterial
        {
            get {return new PopulationSquare(ProductionType.Material, true);}
        }

        protected static PopulationSquare Any
        {
            get {return new PopulationSquare(ProductionType.Any, false);}
        }

        private static ISectorKey Sector(int id, string name)
        {
            return new SectorCreationContext(id, name);
        }

        public class SectorCreationContext : ISectorKey, ISectorVp, ISectorPopulation
        {
            private readonly Sector _sector;

            public SectorCreationContext(int id, string name)
            {
                _sector = new Sector { Id = id, Name = name };
            }

            public ISectorVp Vp(int vp)
            {
                _sector.Vp = vp;
                return this;
            }

            public ISectorVp Ancient(int count = 1)
            {
                _sector.Ancients = count;
                if (count > 0)
                    _sector.HasDiscovery = true;
                return this;
            }

            public ISectorVp Discovery()
            {
                _sector.HasDiscovery = true;
                return this;
            }

            public ISectorVp Artifact()
            {
                _sector.HasArtifact = true;
                return this;
            }

            public ISectorPopulation Population(params PopulationSquare[] populationSquares)
            {
                _sector.Squares = populationSquares.ToList();
                populationSquares.Each(sq => sq.Sector = _sector);
                return this;
            }

            public Sector Wormholes(params int[] facings)
            {
                _sector.Wormholes = facings;
                return _sector;
            }
        }

        public interface ISectorKey
        {
            ISectorVp Vp(int vp);
        }

        public interface ISectorVp 
        {
            ISectorVp Ancient(int count = 1);
            ISectorVp Discovery();
            ISectorVp Artifact();
            ISectorPopulation Population(params PopulationSquare[] populationSquares);
        }

        public interface ISectorPopulation 
        {
            Sector Wormholes(params int[] facings);
        }

    }
}
