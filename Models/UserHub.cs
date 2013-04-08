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
using SpeakingMania.DataLayer;

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
            var romm = RoomStore.FindByKey("MAIN");
            var conn = new Connection() { ConnectionId = Context.ConnectionId, Room = romm };
            ConnectionStore.Add(conn);
            return base.OnConnected();
        }
        public override Task OnDisconnected()
        {
            var us = ConnectionStore.FindById(Context.ConnectionId);
            if (us != null)
            {
                ConnectionStore.Remove(us);
                UpdateUsers(us.Room.RoomIdentity);
            }
            return base.OnDisconnected();
        }
        public void Login(UserProfile userProfile)
        {
            var conn = ConnectionStore.FindById(Context.ConnectionId);
            conn.UserProfile = userProfile;
            ConnectionStore.Update(conn);
        }
        public void JoinRoom(string roomKey)
        {
            var room = RoomStore.FindByKey(roomKey);
            var conn = ConnectionStore.FindById(Context.ConnectionId);
            Groups.Add(Context.ConnectionId, roomKey);
            Clients.Client(conn.ConnectionId).OnJoinRoom(conn.ConnectionId, roomKey);
            conn.Room = room;
            ConnectionStore.Update(conn);
            UpdateUsers(room.RoomIdentity);
            UpdateRooms();
        }
        public void UpdateUsers(string roomKey)
        {
            var simpleUsers = new List<SimpleUser>();
            var users = ConnectionStore.FindByRoomKey(roomKey);
            foreach (var u in users)
            {
                SimpleUser user = new SimpleUser();
                user.RoomId = u.Room.Id;
                user.UserIdentity = u.ConnectionId;
                simpleUsers.Add(user);
            }

            Clients.Group(roomKey).OnUpdateUsers(users);
        }
        public void UpdateRooms()
        {
            var rooms = RoomStore.Rooms.ToList();
            var simpleRooms = new List<SimpleRoom>();
            foreach (var r in rooms)
            {
                SimpleRoom room;
                room.RoomIdentity = r.RoomIdentity;
                room.RoomName = r.RoomName;
                simpleRooms.Add(room);
            }
            Clients.All.OnUpdateRooms(rooms);
            
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