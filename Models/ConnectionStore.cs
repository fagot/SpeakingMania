using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpeakingMania.DAL;

namespace SpeakingMania.Models
{
    public static class ConnectionStore
    {
        private static UnitOfWork _unitOfWork;
        private static List<Connection> _connectionStore;
        public static List<Connection> Connections
        {
            get
            {
                if (_connectionStore == null)
                {
                    //_connectionStore = _unitOfWork.ConnectionRepository.Get(orderBy: q => q.OrderBy(d => d.Id)).ToList();
                    _connectionStore = new List<Connection>();
                    return _connectionStore;

                }
                else
                {
                    return _connectionStore;
                }
            }
        }
        static ConnectionStore()
        {
            _unitOfWork = UoFFactory.UnitOfWork;
        }

        public static UnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        public static void Add(Connection conn)
        {
            Connections.Add(conn);
           // _unitOfWork.ConnectionRepository.Insert(conn);
            //_unitOfWork.Save();
        }
        public static void Remove(Connection conn)
        {

               //_unitOfWork.ConnectionRepository.Delete(conn);
               Connections.Remove(conn);
              // _unitOfWork.Save();

        }

        public static void Update(Connection conn)
        {
            var obj = Connections.First(r => r.Id == conn.Id);
            if (obj != null)
            {
                obj = conn;
                //_unitOfWork.ConnectionRepository.Update(conn);
                //_unitOfWork.Save();
            }
        }

        public static Connection GetById(string identity)
        {
            var cn = Connections.FirstOrDefault(c => c.ConnectionId == identity);
            return cn;
        }
        public static bool IdentityExists(string identity)
        {
            var cn = Connections.Exists(c => c.ConnectionId == identity);
            return cn;
        }
        public static List<Connection> FindByRoomId(int roomId)
        {
            var users = Connections.Where(c => c.RoomId == roomId).ToList();
            return users.ToList();
        }
        public static List<Connection> FindByRoomKey(string roomKey)
        {
            var users = Connections.Where(c => c.Room.RoomIdentity == roomKey).ToList();
            return users.ToList();
        }
        public static Connection FindByUserId(int userId)
        {
            var us = Connections.FirstOrDefault(c => c.UserId == userId);
            return us;
        }
    }

}