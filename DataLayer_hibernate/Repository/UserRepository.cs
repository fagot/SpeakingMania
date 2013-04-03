using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Criterion;
using SpeakingMania.DataLayer.Models;

namespace SpeakingMania.DataLayer.Repository
{
    public class UserRepository:BaseRepository<UserProfile>
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
        public UserProfile GetUserById(int Id)
        {
            UserProfile user;
            lock (Session)
            {
                using (var transaction = Session.BeginTransaction())
                {
                    user = Session
                        .CreateCriteria(typeof (UserProfile))
                        .Add(Restrictions.Eq("Id", Id))
                        .UniqueResult<UserProfile>();
                    transaction.Commit();
                }
            }
            return user;
        }
        public UserProfile GetUserByIdentity(string identity)
        {
            UserProfile user;
            lock (Session)
            {
                using (var transaction = Session.BeginTransaction())
                {
                    user = Session
                        .CreateCriteria(typeof (UserProfile))
                        .Add(Restrictions.Eq("UserIdentity", identity))
                        .UniqueResult<UserProfile>();
                    transaction.Commit();
                }
            }
            return user;
        }

        public UserProfile GetUserByName(string name)
        {
            UserProfile user;
            lock (Session)
            {
                using (var transaction = Session.BeginTransaction())
                {
                    user = Session
                        .CreateCriteria(typeof(UserProfile))
                        .Add(Restrictions.Eq("UserName", name))
                        .UniqueResult<UserProfile>();
                    transaction.Commit();
                }
            }
            return user;
        }

    }
}