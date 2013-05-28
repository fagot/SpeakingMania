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
            if (!(Context.QueryString["clientId"] == null))
            {
                //clientId passed from application
                clientId = Context.QueryString["clientId"].ToString();
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

            var clientId = GetClientId();
            var romm = RoomStore.FindByName("MAIN");
            var us = ConnectionStore.FindById(clientId);
            if (us == null)
            {
                var conn = new Connection() {ConnectionId = clientId, RoomId = romm.Id, Room = romm};
                ConnectionStore.Add(conn);
                //UpdateUsers(romm.Id.ToString());
            }
            return base.OnConnected();
        }
        public override Task OnReconnected()
        {
            var clientId = GetClientId();
            var room = RoomStore.FindByName("MAIN");
            var us = ConnectionStore.FindById(clientId);
            if (us == null)
            {
                var conn = new Connection() { ConnectionId = clientId, RoomId = room.Id, Room = room };
                ConnectionStore.Add(conn);
                UpdateUsers(room.RoomIdentity);
            }
            return base.OnReconnected();
        }
        public override Task OnDisconnected()
        {
            var clientId = GetClientId();
            var us = ConnectionStore.FindById(clientId);
            if (us != null)
            {
                var room = us.Room;
                ConnectionStore.Remove(us);
                UpdateUsers(room.RoomIdentity);
            }
            return base.OnDisconnected();
        }
        
        public void JoinRoom(string roomKey)
        {
            var clientId = GetClientId(); 
            var room = RoomStore.FindByKey(roomKey);
            var conn = ConnectionStore.FindById(clientId);
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