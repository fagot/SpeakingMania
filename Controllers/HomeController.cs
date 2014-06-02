using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SpeakingMania.DAL;
using System;
using SpeakingMania.Models;

namespace SpeakingMania.Controllers
{
    public class HomeController : Controller
    {
        private string _userId;  
        public ActionResult Index()
        {
            var loginCook = Request.Cookies["login"];
            var keyCook = Request.Cookies["identity"];

            if (loginCook != null && keyCook != null && !String.IsNullOrEmpty(keyCook.Value))
            {
                ViewBag.Login = HttpUtility.UrlDecode(loginCook.Value);
                ViewBag.MyKey = keyCook.Value;
            }
            

            return View("Index");
        }
        public ActionResult About()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SaveUserData(string userId)
        {
            try
            {
                ViewBag.UserId = userId;
            }
            catch (Exception)
            {
                return Json(new {success = false});
            }
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult JoinRoom(string userId, string roomName = null, string roomKey = null)
        {
            /*var errors = new Dictionary<string, string>();
            Room room = null;
            var user = ConnectionStore.FindById(userId);
            JsonResult result = null;
            try
            {
                if (roomName != null)
                {
                    room = RoomStore.Add(roomName, user);
                    Session.Add("isRoomOwner", true);
                }
                else if (roomKey != null)
                {
                    room = RoomRepository.Instance.GetRoomByRoomKey(roomKey);
                    room.Users.Add(user);
                    user.Room = room;
                    ConnectionStore.Update(user);
                    Session.Add("isRoomOwner", false);
                }               
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
            }*/
            return null;

        }
        public ActionResult Room()
         {
             /*var roomKey = Session["roomKey"].ToString();
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
                 ViewBag.UserName = user.UserName;
                 if (isOwner)
                 {
                     
                 }
             }*/
             return View("Room");
         }
        [HttpPost]
        public ActionResult Login(string inputLogin, string inputPassword)
        {
            Models.User.LogIn(Request, Response, Session, inputLogin, inputPassword);
            return View("Index");
        }
        [HttpPost]
        public ActionResult Register(string inputEmail, string inputLoginRegister, string inputPasswordRegister, string userId)
        {
            Models.User.Register(Request, Response, inputEmail, inputLoginRegister, inputPasswordRegister);
            return RedirectToAction("Index");
        }
    }
}
