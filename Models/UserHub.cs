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
using SpeakingMania.DAL;

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
            var romm = RoomStore.FindByName("MAIN");
            var conn = new Connection() { ConnectionId = Context.ConnectionId, RoomId = romm.Id};
            ConnectionStore.Add(conn);
            return base.OnConnected();
        }
        public override Task OnDisconnected()
        {
            var us = ConnectionStore.FindById(Context.ConnectionId);
            if (us != null)
            {
                var roomId = us.RoomId;
                ConnectionStore.Remove(us);
                UpdateUsers(roomId);
            }
            return base.OnDisconnected();
        }
        public void Login(UserProfile userProfile)
        {
            var conn = ConnectionStore.FindById(Context.ConnectionId);
            conn.UserId = userProfile.Id;
            ConnectionStore.Update(conn);
        }
        public void JoinRoom(int roomId)
        {
            var room = RoomStore.FindById(roomId);
            var conn = ConnectionStore.FindById(Context.ConnectionId);
            Groups.Add(Context.ConnectionId, roomId.ToString());
            Clients.Client(conn.ConnectionId).OnJoinRoom(conn.ConnectionId, roomId);
            conn.RoomId = room.Id;
            ConnectionStore.Update(conn);
            UpdateUsers(room.Id);
            UpdateRooms();
        }
        public void UpdateUsers(int roomId)
        {
            var simpleUsers = new List<SimpleUser>();
            var users = ConnectionStore.FindByRoomId(roomId);
            foreach (var u in users)
            {
                SimpleUser user = new SimpleUser();
                user.RoomId = u.RoomId;
                user.UserIdentity = u.ConnectionId;
                simpleUsers.Add(user);
            }

            Clients.Group(roomId.ToString()).OnUpdateUsers(simpleUsers);
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