using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Criterion;
using SpeakingMania.DataLayer.Models;

namespace SpeakingMania.DataLayer.Repository
{
    public class RoomRepository : BaseRepository<Room>
    {
        private static RoomRepository _instance ;
        public static RoomRepository Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                else
                {
                    _instance = new RoomRepository();
                    return _instance;
                }

            }
        }
        public Room GetRoomByRoomName(string name)
        {
            Room registration = Session
                    .CreateCriteria(typeof(Room))
                    .Add(Restrictions.Eq("RoomName", name))
                    .UniqueResult<Room>();
            return registration;
        }
        public Room GetRoomByRoomKey(string key)
        {
            Room room = Session
                    .CreateCriteria(typeof(Room))
                    .Add(Restrictions.Eq("RoomIdentity", key))
                    .UniqueResult<Room>();
            return room;
        }
    }
}