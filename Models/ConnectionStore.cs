using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpeakingMania.DataLayer.Models;
using SpeakingMania.DataLayer.Repository;

namespace SpeakingMania.Models
{
    public static class ConnectionStore
    {
        private static List<Connection> _connectionStore;
        public static List<Connection> Connections
        {
            get
            {
                if (_connectionStore == null)
                {
                    _connectionStore = ConnectionRepository.Instance.GetAll();
                    return _connectionStore;
                }
                else
                {
                    return _connectionStore;
                }
            }
        } 

        public static bool CheckUserName(string userName)
        {
            if (                    _connectionStore = ConnectionRepository.Instance.GetAll();
.Instance.GetUserByName(userName) != null)
                return true;
            else
                return false;
        }
        public static void Add(Connection user)
        {
            Users.Add(user);
            UserRepository.Instance.Add(user);
        }
        public static void Remove(Connection user)
        {
            Users.Remove(user);
            UserRepository.Instance.Remove(user);
        }

        public static void Update(Connection user)
        {
            var us = Users.FirstOrDefault(u => u.UserIdentity == user.UserIdentity);
            us = user;
            UserRepository.Instance.Update(us);
        }

        public static Connection FindById(string identity)
        {
            var us = Users.FirstOrDefault(u => u.UserIdentity == identity);
            return us;
        }

        public static List<Connection> FindByRoomKey(string roomKey)
        {
            var users = Users.Where(u => u.Room.RoomIdentity == roomKey).ToList();
            return users.ToList();
        }

        public static Connection FindByName(string userName)
        {
            var us = Users.FirstOrDefault(u => u.UserName == userName);
            return us;
        }
    }
}