using System.Web.Mvc;
using Slugburn.Obscura.Lib;

namespace Slugburn.Obscura.Controllers
{
    public class MainController : Controller
    {
        public MainController(GameFactory gameFactory, ILog log)
        {
        }

        //
        // GET: /Main/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Main/Test

        public ActionResult Test()
        {
            return View();
        }

        //
        // GET: /Main/Game

        public ActionResult Game()
        {
            return View();
        }



    }
}
