using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SpeakingMania.Models
{
    public class RoomHub : Hub
    {
        public void UserConnected(string userId)
        {

        }
    }
}