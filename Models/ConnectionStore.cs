using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SpeakingMania.DataLayer;

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
                    using (var ctx = new SpeakingManiaEntities())
                    {
                        _connectionStore = ctx.Connection.ToList();
                        return _connectionStore;
                    }
                }
                else
                {
                    return _connectionStore;
                }
            }
        } 
        public static void Add(Connection conn)
        {
            using (var ctx = new SpeakingManiaEntities())
            {
                Connections.Add(conn);
                ctx.Connection.Add(conn);
            }
        }
        public static void Remove(Connection conn)
        {
            using (var ctx = new SpeakingManiaEntities())
            {
                Connections.Remove(conn);
                ctx.Connection.Remove(conn);
            }
        }

        public static void Update(Connection conn)
        {
            using (var ctx = new SpeakingManiaEntities())
            {
                var cn = Connections.FirstOrDefault(c => c.ConnectionId == conn.ConnectionId);
                cn = conn;
                ctx.Connection.Attach(conn);
            }
        }

        public static Connection FindById(string identity)
        {
            var cn = Connections.FirstOrDefault(c => c.ConnectionId == identity);
            return cn;
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