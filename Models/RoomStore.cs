using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpeakingMania.DataLayer.Models;
using SpeakingMania.DataLayer.Repository;

namespace SpeakingMania.Models
{
    public static class RoomStore
    {
        private static List<Room> _roomStore;
        public static List<Room> Rooms
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
        public static bool CheckRoomName(string roomName)
        {
            if (RoomRepository.Instance.GetRoomByRoomName(roomName) != null)
                return true;
            else
                return false;
        }
        public static void Remove(Room room)
        {
            Rooms.Remove(room);
            RoomRepository.Instance.Remove(room);
        }

        public static void Update(Room room)
        {
            var rm = Rooms.FirstOrDefault(u => u.RoomIdentity == room.RoomIdentity);
            rm = room;
            RoomRepository.Instance.Update(rm);
        }
        public static Room Add(string roomName, UserProfile user)
        {
            if (user != null)
            {
                var room = new Room
                {
                    RoomIdentity = Guid.NewGuid().ToString("N"),
                    RoomName = roomName,
                    RoomOwner = user,
                    Users = new List<UserProfile>()
                };
                room.Users.Add(user);
                if (RoomRepository.Instance.GetRoomByRoomName(roomName) == null)
                {
                    RoomRepository.Instance.Add(room);
                    Rooms.Add(room);
                    user.Room = room;
                    UserRepository.Instance.Update(user);
                    return room;
                }
                else
                {
                    throw new RoomCreatingException("room with name \"" + room.RoomName + "\" is already exist");
                }


            }
            else
            {
                throw new Exception("User is not found in the DB");
            }
        }

        public static Room FindById(string identity)
        {
            var room = Rooms.FirstOrDefault(r => r.RoomIdentity == identity);
            return room;
        }
    }
}