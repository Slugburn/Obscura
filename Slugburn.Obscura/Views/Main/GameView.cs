using System.Linq;
using Microsoft.AspNet.SignalR;
using Slugburn.Obscura.Lib;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Views.Main
{
    public class GameView : IGameView
    {
        public void Display(Game game)
        {
            var model = game.Map.GetSectors().Select(CreateSectorModel);
            var context = GlobalHost.ConnectionManager.GetHubContext<MainHub>();
            context.Clients.All.displayMap(model);
            context.Clients.All.updateRound(game.Round);
        }

        public void UpdateSector(Sector sector)
        {
            var model = CreateSectorModel(sector);
            var context = GlobalHost.ConnectionManager.GetHubContext<MainHub>();
            context.Clients.All.updateSector(model);
        }

        private static object CreateSectorModel(Sector s)
        {
            return new
            {
                x = s.Location.Coord.X,
                y = s.Location.Coord.Y,
                vp = s.Vp,
                color = s.Owner != null ? s.Owner.Color.ToString() : null,
                wormholes = s.Wormholes,
                planets = CreatePlanets(s)
            };
        }

        private static object CreatePlanets(Sector s)
        {
            return s.Squares.GroupBy(x => x.ProductionType)
                    .Select(
                        g =>
                        new
                        {
                            type = g.Key.ToString(),
                            squares = g.Select(x => new { advanced = x.Advanced, color = x.Owner == null ? null : x.Owner.Color.ToString() })
                        }).ToArray();
        }
    }
}