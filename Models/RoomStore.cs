using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpeakingMania.DataLayer;

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
                    using (var ctx = new SpeakingManiaEntities())
                    {
                        _roomStore = ctx.Room.ToList();
                        return _roomStore;
                    }
                }
                else
                {
                    return _roomStore;
                }
            }
        } 
        public static bool CheckRoomName(string roomName)
        {
            if (Rooms.First(r=>r.RoomName==roomName)!=null)
                return true;
            else
                return false;
        }
        public static void Remove(Room room)
        {
            using (var ctx = new SpeakingManiaEntities())
            {
                Rooms.Remove(room);
                ctx.Room.Remove(room);
            }
        }
        
        public static void Update(Room room)
        {
            using (var ctx = new SpeakingManiaEntities())
            {
                var rm = Rooms.FirstOrDefault(u => u.RoomIdentity == room.RoomIdentity);
                rm = room;
                ctx.Room.Attach(rm);
            }
        }
        public static void Add(Room room)
        {
            using (var ctx = new SpeakingManiaEntities())
            {
                Rooms.Add(room);
                ctx.Room.Add(room);
            }

        }

        public static Room FindByKey(string identity)
        {
            var room = Rooms.FirstOrDefault(r => r.RoomIdentity == identity);
            return room;
        }
    }
}