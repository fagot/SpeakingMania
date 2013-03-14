using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Criterion;
using SpeakingMania.DataLayer.Models;

namespace SpeakingMania.DataLayer.Repository
{
    public class UserRepository:BaseRepository<User>
    {
        private static UserRepository _instance;
        public static UserRepository Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                else
                {
                    _instance = new UserRepository();
                    return _instance;
                }

            }
        }
        public User GetUserById(int Id)
        {
            var user = Session
                    .CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("Id", Id))
                    .UniqueResult<User>();
            return user;
        }
        public User GetUserByIdentity(string identity)
        {
            var user = Session
                    .CreateCriteria(typeof(User))
                    .Add(Restrictions.Eq("UserIdentity", identity))
                    .UniqueResult<User>();
            return user;
        }

    }
}