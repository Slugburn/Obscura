using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slugburn.Obscura.Lib
{
    public class Player
    {
        public string Name { get; set; }

        public Ship Interceptor { get; set; }

        public Ship Cruiser { get; set; }

        public Ship Dreadnaught { get; set; }

        public Ship Starbase { get; set; }

        public int Money { get; set; }

        public int Science { get; set; }

        public int Materials { get; set; }

    }
}
