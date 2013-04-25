using System;
using System.Linq;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Slugburn.Obscura.Lib;
using Slugburn.Obscura.Lib.Maps;
using Slugburn.Obscura.Lib.Ships;

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

        public void MoveShip(Ship ship, Sector start, Sector end)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MainHub>();
            context.Clients.All.updateSector(CreateSectorModel(start));
            context.Clients.All.moveShip(new
                {
                    owner = ship.Owner.ToString(),
                    type = ship.ShipType.ToString(),
                    start = new {x = start.Location.Coord.X, y = start.Location.Coord.Y},
                    end = new {x = end.Location.Coord.X, y = end.Location.Coord.Y},
                });
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
            context.Clients.All.updateSector(CreateSectorModel(end));
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
                planets = CreatePlanets(s),
                ships = CreateShips(s)
            };
        }

        private static object CreateShips(Sector sector)
        {
            return sector.Ships.GroupBy(s => s.Owner.ToString())
                         .Select(
                             og => new
                                       {
                                           owner = og.Key,
                                           types = og.GroupBy(x => x.ShipType.ToString())
                                                     .Select(tg => new {type = tg.Key, count = tg.Count()})
                                       });
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