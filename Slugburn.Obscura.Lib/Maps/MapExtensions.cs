using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura.Lib.Maps
{
    public static class MapExtensions
    {
        public static ProductionType[] GetProductionTypes(this ProductionType productionType)
        {
            switch (productionType)
            {
                case ProductionType.Orbital:
                    return new[] {ProductionType.Money, ProductionType.Science,};
                case ProductionType.Any:
                    return new[] {ProductionType.Material, ProductionType.Money, ProductionType.Science,};
                default:
                    return new[] {productionType};
            }
        }
    }
}
