using System.Web.Mvc;
using SpeakingMania.DataLayer.Repository;
using SpeakingMania.DataLayer.Models;
using System;
using SpeakingMania.Models;

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
            string roomName = "TESt";
            var hub = new UserHub();
            var room = new Room { RoomIdentity = roomName, RoomName = roomName };
            RoomRepository.Instance.Add(room);
            hub.UpdateUsers(roomName);
            return View("Room");
        }

        public ActionResult Login()
        {
            return View("Index");
        }
    }
}
