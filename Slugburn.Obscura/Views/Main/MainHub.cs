using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Slugburn.Obscura.Views.Main
{
    [HubName("main")]
    public class MainHub : Hub
    {
        private readonly GameFactory _gameFactory;

        public MainHub(GameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public void Start()
        {
            var game = _gameFactory.Create();
            game.Start();

        }

    }
}