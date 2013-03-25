using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.UI;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using SpeakingMania.DataLayer.Models;
using SpeakingMania.DataLayer.Repository;

namespace SpeakingMania.Models
{
    public class UserHub : Hub
    {
        #region Properties
        public UserHub()
        {
            
        }

        #endregion
        public override Task OnConnected()
        {
            var romm = RoomRepository.Instance.GetRoomByRoomKey("MAIN");
            var user = new User { UserIdentity = Context.ConnectionId, UserName = "", Room = romm };
            UserStore.Add(user);
            return base.OnConnected();
        }
        public override Task OnDisconnected()
        {
            var us = UserStore.FindById(Context.ConnectionId);
            if (us != null)
            {
                UserStore.Remove(us);
                UpdateUsers(us.Room.RoomIdentity);
            }
            return base.OnDisconnected();
        }
        public void Login(string name)
        {
            var user = UserStore.FindById(Context.ConnectionId);
            user.UserName = name;
            UserStore.Update(user);
        }
        public void JoinRoom(string roomKey)
        {
            var room = RoomRepository.Instance.GetRoomByRoomKey(roomKey);
            var user = UserStore.FindById(Context.ConnectionId);
            Groups.Add(Context.ConnectionId, roomKey);
            Clients.Client(user.UserIdentity).OnJoinRoom(user.UserIdentity);
            user.Room = room;
            UserRepository.Instance.Update(user);
            UpdateUsers(room.RoomIdentity);
        }
         public static User AddUser(string userName)
         {
             var user = new User {UserName = userName};
             UserStore.Add(user);
             return user;
         }
       
        public void UpdateUsers(string roomKey)
        {
            var simpleUsers = new List<SimpleUser>();
            var users = UserStore.FindByRoomKey(roomKey);
            foreach (var u in users)
            {
                SimpleUser user;
                user.RoomId = u.Room.Id;
                user.UserName = u.UserName;
                user.UserIdentity = u.UserIdentity;
                simpleUsers.Add(user);
            }

            Clients.Group(roomKey).OnUpdateUsers(simpleUsers);
        }
        public void UpdateRooms()
        {
            var rooms = RoomRepository.Instance.GetAll();
            var simpleRooms = new List<SimpleRoom>();
            foreach (var r in rooms)
            {
                SimpleRoom room;
                room.RoomIdentity = r.RoomIdentity;
                room.RoomName = r.RoomName;
                simpleRooms.Add(room);
            }
            Clients.All.OnUpdateRooms(simpleRooms);
            
        }
    }

    struct SimpleUser
    {
        public string UserName;
        public string UserIdentity;
        public int RoomId;
    }

    struct SimpleRoom
    {
        public string RoomIdentity;
        public string RoomName;
    }
}