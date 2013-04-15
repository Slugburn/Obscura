using Slugburn.Obscura.Lib.Builders;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Lib.Ai
{
    public class BuildLocation
    {
        public IBuilder Builder { get; set; }

        public Sector Location { get; set; }
    }
}