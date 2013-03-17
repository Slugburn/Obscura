using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slugburn.Obscura.Lib
{
    public interface IFaction
    {
        void Setup(Player player);
        PlayerColor Color { get; }
        string Name { get; }
        int HomeSectorId { get; }
    }
}
