using System.Linq;
using Ninject;
using Slugburn.Obscura.Lib;
using Slugburn.Obscura.Lib.Factions;

namespace Slugburn.Obscura
{
    public class GameFactory
    {
        private readonly IKernel _kernel;

        public GameFactory(IKernel kernel)
        {
            _kernel = kernel;
        }

        public Game Create()
        {
            var game = _kernel.Get<Game>();
            var factions = Enumerable.Range(0, 6).Select(x => _kernel.Get<Faction>()).ToArray();
            game.Setup(factions);
            return game;
        }
    }
}