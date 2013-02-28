using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using SpeakingMania.DataLayer.Repository;

namespace SpeakingMania.Models
{
    public class UserHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }
        public void UpdateUsers(string roomName)
        {
            var mainRoom = RoomRepository.Instance.GetRoomByRoomKey(roomName);
            Clients.All.OnUpdateUsers(mainRoom.Users);
        }
    }
}