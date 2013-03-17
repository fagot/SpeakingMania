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
        public ActionResult JoinRoom(string userId, string roomName = null, string roomKey = null)
        {
            var errors = new Dictionary<string, string>();
            var user = UserRepository.Instance.GetUserByIdentity(userId);
            if (user != null)
            {
                Session.Add("userId", user.UserIdentity);
                if (roomName != null)
                {
                    Session.Add("isRoomOwner", true);
                    var room = new Room
                    {
                        RoomIdentity = Guid.NewGuid().ToString("N"),
                        RoomName = roomName,
                        RoomOwner = user,
                        Users = new List<User>()
                    };
                    room.Users.Add(user);
                    if (RoomRepository.Instance.GetRoomByRoomName(roomName) == null)
                    {
                        RoomRepository.Instance.Add(room);
                        Session.Add("roomKey", room.RoomIdentity);
                        return Json(new {success = true, roomName = room.RoomName, roomKey = room.RoomIdentity, userId = user.UserIdentity, isRoomOwner = true});
                    }
                    else
                    {
                        errors.Add("room", "The room with the name \""+room.RoomName+"\" is already exist");
                        return Json(new { success = false, errors });
                    }
                }
                else if (roomKey != null)
                {
                    Session.Add("isRoomOwner", false);
                    Session.Add("roomKey", roomKey);
                }

            }
            else
            {
                throw new Exception("User is not found in the DB");
            } 
            return View("Room");
        }
         public ActionResult Room()
         {
             var roomKey = Session["roomKey"].ToString();
             var userId = Session["userId"].ToString();
             var isOwner = Convert.ToBoolean(Session["isRoomOwner"]);
             if (!String.IsNullOrEmpty(roomKey) && !String.IsNullOrEmpty(userId))
             {
                 ViewBag.UserId = userId;
                 ViewBag.RoomKey = roomKey;
                 ViewBag.IsOwner = isOwner;
                 var user = UserRepository.Instance.GetUserByIdentity(userId);
                 var room = RoomRepository.Instance.GetRoomByRoomKey(roomKey);
                 if (isOwner)
                 {
                     
                 }
             }
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

                return Json(new { success = true, name = login });
                //return Index();
            }
            return Json(new { success = false, errors });
            //return View("Index");
        }
    }
}
