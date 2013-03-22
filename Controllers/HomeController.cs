using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using SpeakingMania.DataLayer.Repository;
using SpeakingMania.DataLayer.Models;
using System;
using SpeakingMania.Models;

namespace SpeakingMania.Controllers
{
    public class HomeController : Controller
    {
        private UserHub hub;
        public ActionResult Index()
        {
            var cook = Request.Cookies["mylogin"];
            var keyCook = Request.Cookies["mykey"];
            var roomKey = Session["roomKey"];
            if (roomKey != null && (string)roomKey != "MAIN")
            {
                //return Room();
            }
            if (cook != null && keyCook != null && !String.IsNullOrEmpty(keyCook.Value))
            {
                ViewBag.Login = HttpUtility.UrlDecode(cook.Value);
                ViewBag.MyKey = keyCook.Value;
                //var user = UserRepository.Instance.GetUserByIdentity(keyCook.Value);
                //if (user.Room.RoomName != "MAIN")
                //{
                //    return Room();
                //}
                
            }
            

            return View("Index");
        }
        public ActionResult About()
        {
            return View();
        }
        [HttpPost]
        public JsonResult JoinRoom(string userId, string roomName = null, string roomKey = null)
        {
            var errors = new Dictionary<string, string>();
            Room room = null;
            JsonResult result = null;
            try
            {
                if (roomName != null)
                {
                    room = UserHub.CreateRoom(roomName, userId);
                }
                else if (roomKey != null)
                {
                    room = RoomRepository.Instance.GetRoomByRoomKey(roomKey);
                }
                Session.Add("userId", userId);
                Session.Add("isRoomOwner", true);
                Session.Add("roomKey", room.RoomIdentity);
                result =
                    Json(
                        new
                            {
                                success = true,
                                roomName = room.RoomName,
                                roomKey = room.RoomIdentity,
                                userId = userId,
                                isRoomOwner = true
                            });
            }
            catch (RoomCreatingException ex)
            {
                errors.Add("room", ex.Message);
                result = Json(new {success = false, errors = errors});
            }
            return result;

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
                 ViewBag.RoomName = room.RoomName;
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
