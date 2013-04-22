using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Slugburn.Obscura.Lib;
using Slugburn.Obscura.Lib.Maps;

namespace Slugburn.Obscura.Controllers
{
    public class MainController : Controller
    {
        private readonly GameFactory _gameFactory;
        private readonly ILog _log;
        private readonly IMapVisualizer _mapVisualizer;

        public MainController(GameFactory gameFactory, ILog log, IMapVisualizer mapVisualizer)
        {
            _gameFactory = gameFactory;
            _log = log;
            _mapVisualizer = mapVisualizer;
        }

        //
        // GET: /Main/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Main/Test

        public JsonResult Test()
        {
            var game = _gameFactory.Create();
            game.Start();

            var result = GetTestResults().ToArray();

            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<object> GetTestResults()
        {
            var visualizer = (CaptureMapVisualizer) _mapVisualizer;
            return visualizer.Map.GetSectors()
                .Select(CreateSectorView);
        }

        private static object CreateSectorView(Sector s)
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
                                squares = g.Select(x => new {advanced = x.Advanced, color = x.Owner == null ? null : x.Owner.Color.ToString()})
                            }).ToArray();
        }
    }
}
