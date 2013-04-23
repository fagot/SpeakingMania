using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;

namespace SpeakingMania.DataLayer
{
    public static class Database
    {
        private static ISessionFactory _sessionFactory;
        private static object _sync = new object();
        private static ISessionFactory SessionFactory
        {
            get
            {
                lock (_sync)
                {
                    if (_sessionFactory == null)
                    {
                        var configuration = new Configuration();

                        configuration.Configure();
                        //configuration.AddAssembly("SpeakingMania");



                        _sessionFactory = configuration.BuildSessionFactory();
                    }
                }
                return _sessionFactory;
            }
        }

        public static ISession GetSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}