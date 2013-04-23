using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpeakingMania.DAL
{
    public class UnitOfWork : IDisposable
    {
        public UnitOfWork()
        {
            context = new SpeakingManiaContext();
        }

        private SpeakingManiaContext context;
        private GenericRepository<Connection> _connectionRepository;
        private GenericRepository<Room> _roomRepository;
        private GenericRepository<UserProfile> _userProfileRepository;

        public GenericRepository<Connection> ConnectionRepository
        {
            get
            {

                if (this._connectionRepository == null)
                {
                    this._connectionRepository = new GenericRepository<Connection>(context);
                }
                return _connectionRepository;
            }
        }

        public GenericRepository<Room> RoomRepository
        {
            get
            {

                if (this._roomRepository == null)
                {
                    this._roomRepository = new GenericRepository<Room>(context);
                }
                return _roomRepository;
            }
        }

        public GenericRepository<UserProfile> UserProfileRepository
        {
            get
            {

                if (this._userProfileRepository == null)
                {
                    this._userProfileRepository = new GenericRepository<UserProfile>(context);
                }
                return _userProfileRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}