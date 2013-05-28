using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpeakingMania.DAL;

namespace SpeakingMania.Models
{
    public static class RoomStore
    {
        private static UnitOfWork _unitOfWork;
        private static List<Room> _roomStore;
        public static List<Room> Rooms
        {
            get
            {
                if (_roomStore == null)
                {
                    using (var ctx = new SpeakingManiaContext())
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
        static RoomStore()
        {
            _unitOfWork = UoFFactory.UnitOfWork;
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
                Rooms.Remove(room);
                _unitOfWork.RoomRepository.Delete(room);  
        }
        
        public static void Update(Room room)
        {
            var obj = Rooms.First(r => r.Id == room.Id);
            if (obj != null)
            {
                obj = room;
                _unitOfWork.RoomRepository.Update(room);
                _unitOfWork.Save();
            }
        }
        public static void Add(Room room)
        {
            Rooms.Add(room);
            _unitOfWork.RoomRepository.Insert(room);
            _unitOfWork.Save();

        }
        public static Room FindByName(string name)
        {
            var room = Rooms.FirstOrDefault(r => r.RoomName == name);
            return room;
        }
        public static Room FindById(int id)
        {
            var room = Rooms.FirstOrDefault(r => r.Id == id);
            return room;
        }
        public static Room FindByKey(string key)
        {
            var room = Rooms.FirstOrDefault(r => r.RoomIdentity == key);
            return room;
        }
    }
}