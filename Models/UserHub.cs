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
            var cc = Context.RequestCookies.FirstOrDefault(c=>c.Value.ToString()=="connection");
            if (cc.Value == null)
            {
                Context.RequestCookies.Add("connection", new Cookie("connection", Context.ConnectionId));
            }
            else
            {
                var us = ConnectionStore.FindById(cc.Value.ToString());
                if (us != null)
                {
                    ConnectionStore.Remove(us);
                } 
            }
            var romm = RoomStore.FindByName("MAIN");
            var conn = new Connection() { ConnectionId = Context.ConnectionId, RoomId = romm.Id, Room = romm };
            ConnectionStore.Add(conn);
            //CheckList();
            return base.OnConnected();
        }
        public override Task OnDisconnected()
        {
            var us = ConnectionStore.FindById(Context.ConnectionId);
            if (us != null)
            {
                var roomId = us.RoomId;
                ConnectionStore.Remove(us);
                UpdateUsers(roomId.ToString());
            }
            return base.OnDisconnected();
        }
        private void CheckList()
        {
            foreach (var connection in ConnectionStore.Connections)
            {
                if (Clients.All[connection.ConnectionId] == null)
                {
                    ConnectionStore.Remove(connection);
                }
            }
        }
        public void Login(UserProfile userProfile)
        {
            var conn = ConnectionStore.FindById(Context.ConnectionId);
            conn.UserId = userProfile.Id;
            ConnectionStore.Update(conn);
        }
        public void JoinRoom(string roomKey)
        {
            var room = RoomStore.FindByKey(roomKey);
            var conn = ConnectionStore.FindById(Context.ConnectionId);
            Groups.Add(Context.ConnectionId, roomKey);
            Clients.Client(conn.ConnectionId).OnJoinRoom(conn.ConnectionId, roomKey);
            conn.RoomId = room.Id;
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
                user.RoomId = u.RoomId;
                user.UserIdentity = u.ConnectionId;
                simpleUsers.Add(user);
            }

            Clients.All.OnUpdateUsers(simpleUsers);
            
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