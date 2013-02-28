using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using SpeakingMania.DataLayer.Models;
using SpeakingMania.DataLayer.Repository;

namespace SpeakingMania.Models
{
    public class UserHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }
        public void JoinRoom(string roomKey, string myName)
        {
            var room = RoomRepository.Instance.GetRoomByRoomKey(roomKey);
            var user = new User {Room = room, UserIdentity = Guid.NewGuid().ToString("N"), UserName = myName};
            room.Users.Add(user);
            Clients.Client(user.UserIdentity).OnJoinRoom(user.UserIdentity);
            UpdateUsers(room.RoomIdentity);
        }
        public void UpdateUsers(string roomKey)
        {
            var room = RoomRepository.Instance.GetRoomByRoomKey(roomKey);
            Clients.Group(roomKey).OnUpdateUsers(room.Users);
        }
    }
}