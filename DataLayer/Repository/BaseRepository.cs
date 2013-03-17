using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Linq;

namespace SpeakingMania.DataLayer.Repository
{
    public class BaseRepository<T> : IRepository<T>
    {
        protected readonly ISession Session;
        public BaseRepository()
        {
            Session = Database.GetSession();
        }
        public IQueryable<T> Query
        {
            get
            {
                try
                {

                    IQueryable<T> query = Session.Query<T>();
                    return query;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        public void Add(T entity)
        {
            try
            {
                lock (Session)
                {
                    using (var transaction = Session.BeginTransaction())
                    {
                        Session.Save(entity);
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Remove(T entity)
        {
            try
            {
                lock (Session)
                {
                    using (var transaction = Session.BeginTransaction())
                    {
                        Session.Delete(entity);
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(T entity)
        {
            try
            {
                lock (Session)
                {
                    using (var transaction = Session.BeginTransaction())
                    {
                        Session.SaveOrUpdate(entity);
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<T> GetAll()
        {
            List<T> list;
            lock (Session)
            {
                using (var transaction = Session.BeginTransaction())
                {
                    list = Session.Query<T>().ToList();
                    transaction.Commit();
                }
            }
            return list;
        }
    }
}