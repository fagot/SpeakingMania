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
        private static List<Room> _roomStore;
        private static List<User> _userStore;
        public List<User> UserStore
        {
            get
            {
                if (_userStore == null)
                {
                    _userStore = UserRepository.Instance.GetAll();
                    return _userStore;
                }
                else
                {
                    return _userStore;
                }
            }
        }
        public List<Room> RoomStore
        {
            get
            {
                if (_roomStore == null)
                {
                    _roomStore = RoomRepository.Instance.GetAll();
                    return _roomStore;
                }
                else
                {
                    return _roomStore;
                }
            }
        }
        #endregion
        public override Task OnDisconnected()
        {
            var us = UserStore.FirstOrDefault(u => u.UserIdentity == Context.ConnectionId);
            if (us != null)
            {
                UserStore.Remove(us);
                UserRepository.Instance.Remove(us);
                UpdateUsers(us.Room.RoomIdentity);
            }
            return base.OnDisconnected();
        }
        public void Send(string name, string message)
        {
            Clients.All.broadcastMessage(name, message);
        }
        public void JoinRoom(string roomKey, string myName)
        {
            var room = RoomRepository.Instance.GetRoomByRoomKey(roomKey);
            var user = UserStore.FirstOrDefault(u => u.UserIdentity == Context.ConnectionId);
            if (user == null)
            {
                user = new User {Room = room, UserIdentity = Context.ConnectionId, UserName = myName};
            }
            UserStore.Add(user);
            Groups.Add(Context.ConnectionId, roomKey);
            Clients.Client(user.UserIdentity).OnJoinRoom(user.UserIdentity);
            UserRepository.Instance.Update(user);
            UpdateUsers(room.RoomIdentity);
        }
        public void CreateRoom(string roomName, string userId)
        {
            var user = UserRepository.Instance.GetUserByIdentity(userId);
            if (user != null)
            {
                if (roomName == null)
                {
                    var room = new Room
                        {
                            RoomIdentity = Guid.NewGuid().ToString("N"),
                            RoomName = roomName,
                            RoomOwner = user
                        };
                    room.Users.Add(user);
                    RoomRepository.Instance.Add(room);
                }
                
            }
            else
            {
                throw new Exception("User is not found in the DB");
            } 
        }
        public void UpdateUsers(string roomKey)
        {
            var simpleUsers = new List<SimpleUser>();
            var users = UserStore.Where(u => u.Room.RoomIdentity == roomKey).ToList();
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
            
        }
    }

    struct SimpleUser
    {
        public string UserName;
        public string UserIdentity;
        public int RoomId;
    }
}