using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slugburn.Obscura.Lib
{
    class SectorCatalog
    {
        public static Dictionary<int, Sector> GetSectors()
        {
            return new[]
                       {
                           Sector(1, "Galactic Center").Vp(4).Artifact().Discovery()
                               .Planet(Science, AdvScience).Planet(Material, AdvMaterial).Planet(Any, Any).Wormholes(0, 1, 2, 3, 4, 5),
                           Sector(101, "Castor").Vp(2).Ancients(1).Planet(Money).Planet(Material, AdvMaterial).Wormholes(1, 2, 3, 4, 5),
                           Sector(102, "Pollux").Vp(3).Artifact().Planet(Science, AdvScience).Wormholes(0, 1, 3, 4),
                           Sector(103, "Beta Leonis").Vp(2).Planet(AdvMoney).Planet(Any).Wormholes(0, 1, 2, 4, 5),
                           Sector(104, "Arcturus").Vp(2).Ancients(2).Planet(Money, AdvMoney).Planet(Science, AdvScience).Wormholes(0, 1, 3, 4),
                           Sector(105, "Zeta Hurculis").Vp(3).Artifact().Ancients(1).Planet(Money).Planet(Science).Planet(AdvMaterial).Wormholes(0, 1, 3, 4, 5),
                           Sector(106, "Capella").Vp(2).Planet(Science).Planet(Material).Wormholes(0, 1, 2, 3),
                           Sector(107, "Aldebaran").Vp(2).Planet(Money).Planet(AdvScience).Planet(AdvMaterial).Wormholes(0, 1, 2, 3, 5),
                           Sector(108, "Mu Cassiopaeiae").Vp(2).Ancients(1).Planet(AdvMoney).Planet(Science).Planet(Any).Wormholes(0, 1, 3, 4),
                           Sector(201, "Alpha Centauri").Vp(1).Planet(Money).Planet(Material).Wormholes(1, 3, 5),
                           Sector(202, "Fomalhaut").Vp(1).Planet(Science, AdvScience).Wormholes(1, 3, 5),
                           Sector(203, "Chi Draconis").Vp(1).Ancients(2).Planet(Money).Planet(Science).Planet(Material).Wormholes(0, 1, 3, 5),
                           Sector(204, "Vega").Vp(2).Artifact().Ancients(1).Planet(AdvMoney).Planet(AdvMaterial).Planet(Any).Wormholes(0, 1, 3, 5),
                           Sector(205, "Mu Herculis").Vp(1).Planet(Money, AdvMoney).Planet(Science).Wormholes(2, 3, 4),
                           Sector(206, "Epsilon Indi").Vp(1).Discovery().Planet(Material).Wormholes(1, 2, 3, 5),
                           Sector(207, "Zeta Reticuli").Vp(1).Discovery().Wormholes(0, 1, 3),
                           Sector(208, "Iota Persei").Vp(1).Discovery().Wormholes(0, 2, 3, 5),
                           Sector(209, "Delta Eridani").Vp(1).Planet(AdvMoney).Planet(Science).Wormholes(0, 1, 3, 5),
                           Sector(210, "Psi Capricorni").Vp(1).Planet(Money).Planet(Material).Wormholes(0, 3, 5),
                           Sector(211, "Beta Aquilae").Vp(2).Artifact().Ancients(1).Planet(Money).Planet(AdvMaterial).Planet(Any).Wormholes(0, 1, 2, 3),
                           Sector(221, "Procyon").Vp(3).Artifact().Planet(Money, AdvMoney).Planet(Science, AdvScience).Planet(Material).Wormholes(0, 1, 3, 4),
                           Sector(222, "Epsilon Eridani").Vp(3).Artifact().Planet(Money, AdvMoney).Planet(Science, AdvScience).Wormholes(0, 1, 3, 4),
                           Sector(223, "Altair").Vp(3).Artifact().Planet(Money, AdvMoney).Planet(Science, AdvScience).Planet(Material).Wormholes(0, 1, 3, 4),
                           Sector(224, "Beta Hydri").Vp(3).Artifact().Planet(Money).Planet(Science, AdvScience).Planet(AdvMaterial).Wormholes(0, 1, 3, 4),
                           Sector(225, "Eta Cassiopeiae").Vp(3).Artifact()
                               .Planet(Money, AdvMoney).Planet(Science, AdvScience).Planet(Material).Wormholes(0, 1, 3, 4),
                           Sector(226, "51 Cygni").Vp(3).Artifact().Planet(Money).Planet(Science).Planet(Material).Wormholes(0, 1, 3, 4),
                           Sector(227, "Sirius").Vp(3).Artifact().Planet(Money, AdvMoney).Planet(Science, AdvScience).Planet(Material).Wormholes(0, 1, 3, 4),
                           Sector(228, "Sigma Draconis").Vp(3).Artifact().Planet(Money).Planet(Science).Planet(AdvMaterial).Wormholes(0, 1, 3, 4),
                           Sector(229, "Tau Ceti").Vp(3).Artifact().Planet(Money, AdvMoney).Planet(Science, AdvScience).Planet(Material).Wormholes(0, 1, 3, 4),
                           Sector(230, "Lambda Aurigae").Vp(3).Artifact().Planet(Money, AdvMoney).Planet(Science).Planet(AdvMaterial).Wormholes(0, 1, 3, 4),
                           Sector(231, "Delta Pavonis").Vp(3).Artifact()
                               .Planet(Money, AdvMoney).Planet(Science, AdvScience).Planet(Material).Wormholes(0, 1, 3, 4),
                           Sector(232, "Rigel").Vp(3).Artifact().Planet(AdvMoney).Planet(Science).Planet(Material, AdvMaterial).Wormholes(0, 1, 3, 4),
                           Sector(301, "Zeta Draconis").Vp(2).Artifact().Ancients(2).Planet(Money).Planet(Science).Planet(AdvMaterial).Wormholes(0, 2, 3),
                           Sector(302, "Gamma Serpentis").Vp(2).Artifact().Ancients(1).Planet(AdvMoney).Planet(Material).Wormholes(0, 3, 4),
                           Sector(303, "Eta Cephei").Vp(2).Artifact().Ancients(1).Planet(Any).Wormholes(3, 5),
                           Sector(304, "Theta Pegasi").Vp(1).Planet(AdvMoney).Planet(Material).Wormholes(0, 3),
                           Sector(305, "Lambda Serpentis").Vp(1).Ancients(1).Planet(Science).Planet(Material).Wormholes(0, 1, 3),
                           Sector(306, "Beta Centauri").Vp(1).Planet(Money).Planet(Material).Wormholes(1, 3),
                           Sector(307, "Sigma Sagittarii").Vp(1).Planet(Money).Planet(AdvScience).Wormholes(0, 2, 3),
                           Sector(308, "Kappa Scorpii").Vp(1).Planet(Science).Planet(AdvMaterial).Wormholes(2, 3, 5),
                           Sector(309, "Phi Piscium").Vp(1).Planet(Money).Planet(AdvScience).Wormholes(0, 3, 5),
                           Sector(310, "Nu Phoenicis").Vp(1).Planet(Science).Planet(Material).Wormholes(0, 3),
                           Sector(311, "Canopus").Vp(1).Discovery().Planet(Material).Wormholes(0, 2, 3),
                           Sector(312, "Antares").Vp(1).Discovery().Planet(Material).Wormholes(0, 1, 3),
                           Sector(313, "Alpha Ursae Minoris").Vp(1).Discovery().Planet(Any).Wormholes(0, 3),
                           Sector(314, "Spica").Vp(1).Discovery().Planet(Any).Wormholes(2, 3, 4),
                           Sector(315, "Epsilon Aurigae").Vp(1).Discovery().Wormholes(0, 3, 5),
                           Sector(316, "Iota Carinae").Vp(1).Discovery().Wormholes(0, 1, 3),
                           Sector(317, "Beta Crucis").Vp(1).Planet(Money, AdvMoney).Wormholes(3, 4),
                           Sector(318, "Gamma Veldrum").Vp(1).Planet(AdvMaterial).Planet(Any).Wormholes(2, 3),
                       }
                .ToDictionary(x => x.Id);
        }

        protected static PopSpace Science
        {
            get { return new PopSpace(ProductionType.Science, false); }
        }

        protected static PopSpace AdvScience
        {
            get { return new PopSpace(ProductionType.Science, true); }
        }

        protected static PopSpace Money
        {
            get { return new PopSpace(ProductionType.Money, false); }
        }

        protected static PopSpace AdvMoney
        {
            get { return new PopSpace(ProductionType.Money, true); }
        }

        protected static PopSpace Material
        {
            get {return new PopSpace(ProductionType.Material, false);}
        }

        protected static PopSpace AdvMaterial
        {
            get {return new PopSpace(ProductionType.Material, true);}
        }

        protected static PopSpace Any
        {
            get {return new PopSpace(ProductionType.Any, false);}
        }

        private static ISectorKey Sector(int id, string name)
        {
            return new SectorCreationContext(id, name);
        }

        public class SectorCreationContext : ISectorKey, ISectorVp, ISectorPlanet
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

            public ISectorVp Ancients(int count)
            {
                _sector.Ancients = count;
                if (count > 0)
                    _sector.Discovery = true;
                return this;
            }

            public ISectorVp Discovery()
            {
                _sector.Discovery = true;
                return this;
            }

            public ISectorVp Artifact()
            {
                _sector.Artifact = true;
                return this;
            }

            public ISectorPlanet Planet(params PopSpace[] populationSpaces)
            {
                _sector.AddPlanet(populationSpaces);
                return this;
            }

            public Sector Wormholes(params int[] wormholes)
            {
                _sector.Wormholes = wormholes;
                return _sector;
            }
        }

        public interface ISectorKey
        {
            ISectorVp Vp(int vp);
        }

        public interface ISectorVp : ISectorVpOrPlanet
        {
            ISectorVp Ancients(int count);
            ISectorVp Discovery();
            ISectorVp Artifact();
        }

        public interface ISectorPlanet : ISectorVpOrPlanet
        {
        }

        public interface ISectorVpOrPlanet
        {
            ISectorPlanet Planet(params PopSpace[] populationSpaces);
            Sector Wormholes(params int[] wormholes);
        }
    }
}
