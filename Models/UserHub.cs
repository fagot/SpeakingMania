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
        private string GetClientId()
        {
            string clientId = "";
            if (!String.IsNullOrEmpty(Context.QueryString["clientId"]))
            {
                //clientId passed from application
                clientId = Context.QueryString["clientId"];
            }

            if (clientId.Trim() == "")
            {
                //default clientId: connectionId
                clientId = Context.ConnectionId;
            }
            return clientId;

        }
        public override Task OnConnected()
        {
            string username = "";
            if (Context.Request.Cookies.ContainsKey("username"))
            {
                username = Context.Request.Cookies["username"].Value;
            }
            else
            {
                username = "anonymous";
            }
            using (var db = new SpeakingManiaContext())
            {
                var defaultRoom = db.Room.SingleOrDefault(s => s.RoomName == "MAIN");
                var user = db.UserProfile
                    //.Include(u => u.Connection)
                    .SingleOrDefault(u => u.UserName == username);

                user.Connection.Add(new Connection
                {
                    ConnectionId = Context.ConnectionId,
                    Connected = true,
                    Room = defaultRoom,
                    RoomId = defaultRoom.Id
                });
                db.SaveChanges();
                UpdateUsers("MAIN");
            }
            return base.OnConnected();
        }
        public override Task OnReconnected()
        {
            //var clientId = GetClientId();
            //return Clients.All.rejoined(Context.ConnectionId, DateTime.Now.ToString());
            //var room = RoomStore.FindByName("MAIN");
            //var us = ConnectionStore.GetById(clientId);
            //if (us == null)
            //{
            //    var conn = new Connection() { ConnectionId = clientId, RoomId = room.Id, Room = room };
            //    ConnectionStore.Add(conn);
            //    UpdateUsers(room.RoomIdentity);
            //}
            return base.OnReconnected();
        }
        public override Task OnDisconnected()
        {
            using (var db = new SpeakingManiaContext())
            {
                var connection = db.Connection.FirstOrDefault(s=>s.ConnectionId == Context.ConnectionId);
                connection.Connected = false;
                db.SaveChanges();
            }
            UpdateUsers("MAIN");
            return base.OnDisconnected();
        }
        
        public void JoinRoom(string roomKey)
        {
            var clientId = GetClientId();
            if (ConnectionStore.IdentityExists(clientId))
            {
                var room = RoomStore.FindByKey(roomKey);
                var conn = ConnectionStore.GetById(clientId);
                Groups.Add(Context.ConnectionId, roomKey);
                Clients.Client(conn.ConnectionId).OnJoinRoom(conn.ConnectionId, roomKey);
                conn.RoomId = room.Id;
                ConnectionStore.Update(conn);
                UpdateUsers(room.RoomIdentity);
            }
            UpdateRooms();
        }
        public void UpdateUsers(string roomKey)
        {
            var simpleUsers = new List<SimpleUser>();
            using (var db = new SpeakingManiaContext())
            {
                var room =
                    db.Room.FirstOrDefault(s => s.RoomIdentity == roomKey);
                var users = db.UserProfile.Where(u => u.Connection.Any(c => c.Connected == true && c.RoomId == room.Id));

                foreach (var u in users)
                {
                    SimpleUser user = new SimpleUser();
                    user.RoomId = room.Id;
                    user.UserIdentity = u.UserIdentity;
                    user.UserName = u.UserName;
                    simpleUsers.Add(user);
                }
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