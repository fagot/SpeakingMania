using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Criterion;
using SpeakingMania.DataLayer.Models;

namespace SpeakingMania.DataLayer.Repository
{
    public class ConnectionRepository:BaseRepository<Connection>
    {
        private static ConnectionRepository _instance;
        public static ConnectionRepository Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                else
                {
                    _instance = new ConnectionRepository();
                    return _instance;
                }

            }
        }
        public Connection GetConnectionById(int Id)
        {
            Connection conn;
            lock (Session)
            {
                using (var transaction = Session.BeginTransaction())
                {
                    conn = Session
                        .CreateCriteria(typeof(Connection))
                        .Add(Restrictions.Eq("Id", Id))
                        .UniqueResult<Connection>();
                    transaction.Commit();
                }
            }
            return conn;
        }
    }
}