using System.Web.Mvc;
using Slugburn.Obscura.Lib;

namespace Slugburn.Obscura.Controllers
{
    public class MainController : Controller
    {
        private readonly GameFactory _gameFactory;
        private readonly ILog _log;

        public MainController(GameFactory gameFactory, ILog log)
        {
            _gameFactory = gameFactory;
            _log = log;
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

            return Json(null, JsonRequestBehavior.AllowGet);
        }

    }
}
