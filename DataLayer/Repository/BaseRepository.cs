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
                Session.Save(entity);
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
                Session.Delete(entity);
                Session.Flush();
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
                Session.SaveOrUpdate(entity);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<T> GetAll()
        {
           return Session.Query<T>().ToList();
        }
    }
}