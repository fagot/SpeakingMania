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
            Room room;
            lock (Session)
            {
                using (var transaction = Session.BeginTransaction())
                {
                    room = Session
                        .CreateCriteria(typeof (Room))
                        .Add(Restrictions.Eq("RoomName", name))
                        .UniqueResult<Room>();
                    transaction.Commit();
                }
            }
            return room;
        }
        public Room GetRoomByRoomKey(string key)
        {

            Room room;
            lock (Session)
            {
                using (var transaction = Session.BeginTransaction())
                {
                    room = Session
                        .CreateCriteria(typeof (Room))
                        .Add(Restrictions.Eq("RoomIdentity", key))
                        .UniqueResult<Room>();
                    transaction.Commit();
                }
            }
            return room;
        }
    }
}