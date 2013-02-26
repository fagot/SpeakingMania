using System.Web.Mvc;

namespace SpeakingMania.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Room()
        {
            return View("Room");
        }
    }
}
