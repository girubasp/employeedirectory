using System;
using NHibernate;
using NHibernate.Context;

namespace DataAccess.Core
{
    public interface ISessionManager : IDisposable
    {
        void Start();
        void Complete();
        void End();

        ISession GetSession();

        ISessionFactory GetSessionFactory();

    }

    public class SessionManager : ISessionManager
    {
        private readonly ISessionFactory _sessionFactory;

        public SessionManager(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public ISession GetSession()
        {
            ISession session;

            if (CurrentSessionContext.HasBind(_sessionFactory))
            {
                session = _sessionFactory.GetCurrentSession();
            }
            else
            {
                session = _sessionFactory.OpenSession();
            }

            return session;
        }

        public void Start()
        {

            var session = _sessionFactory.OpenSession();

            session.FlushMode = FlushMode.Auto;

            CurrentSessionContext.Bind(session);

        }

        public void Complete()
        {

            if (CurrentSessionContext.HasBind(_sessionFactory))
            {
                var session = _sessionFactory.GetCurrentSession();
                session.Flush();
            }

        }

        public void End()
        {
            DisposeSession();
        }

        private void DisposeSession()
        {
            try
            {

                if (CurrentSessionContext.HasBind(_sessionFactory))
                {
                    var session = CurrentSessionContext.Unbind(_sessionFactory);

                    session.Dispose();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Dispose()
        {
            try
            {
                DisposeSession();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _sessionFactory.Dispose();
            }

        }

        public ISessionFactory GetSessionFactory()
        {
            return _sessionFactory;
        }

    }
}
