using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpeakingMania.DAL
{
    public static class UoFFactory
    {
        private static UnitOfWork _uof;
        public static UnitOfWork UnitOfWork
        {
            get
            {
                if (_uof == null)
                {
                    _uof = new UnitOfWork();
                }
                return _uof;

            }
        }
    }
}