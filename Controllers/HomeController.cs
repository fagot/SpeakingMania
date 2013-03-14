using System.Collections.Generic;
using System.Web;
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
            var cook = Request.Cookies["mylogin"];
            var keyCook = Request.Cookies["mykey"];

            if (cook != null && keyCook != null && !String.IsNullOrEmpty(keyCook.Value))
            {
                ViewBag.Login = HttpUtility.UrlDecode(cook.Value);
                ViewBag.MyKey = keyCook.Value;
            }

            return View("Index");
        }

        public ActionResult About()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Room(string userId, string roomKey = null)
        {
           
            return View("Room");
        }
        [HttpPost]
        public ActionResult Login(string login)
        {
            var errors = new Dictionary<string, string>();
            if (!String.IsNullOrEmpty(login) && login.Length < 255)
            {
                var keyCook = Request.Cookies["mykey"];
                if (keyCook == null)
                {
                    keyCook = new HttpCookie("mykey", Guid.NewGuid().ToString("N"));
                    HttpContext.Response.Cookies.Add(keyCook);
                }

                var loginCook = new HttpCookie("mylogin", HttpUtility.UrlEncode(login)) { Expires = DateTime.Now.AddDays(1) };
                HttpContext.Response.Cookies.Add(loginCook);

                //return Json(new { success = true, name = login });
                return Index();
            }
            //return Json(new { success = false, errors });
            return View("Index");
        }
    }
}
