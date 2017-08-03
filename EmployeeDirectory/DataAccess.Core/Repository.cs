using System.Collections.Generic;
using NHibernate;

namespace DataAccess.Core
{
    public class Repository : IRepository
    {
        private readonly ISessionManager _sessionManager;

        public Repository(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public ISession Session
        {
            get { return _sessionManager.GetSession(); }
        }

        public T GetById<T, TId>(TId id)
        {
            return Session.Get<T>(id);
        }

        public void Delete<T>(T entity)
        {
            Session.Delete(entity);
        }

        public virtual T Insert<T>(T entity)
        {
            Session.Save(entity);
            return entity;
        }

        public virtual T Update<T>(T entity)
        {
            Session.Update(entity);
            return entity;
        }

        public IList<T> Find<T>(IQuery<T> query)
        {
            return query.Execute(Session);
        }

        public PagedResult<T> Find<T>(IPagedQuery<T> query)
        {
            return query.Execute(Session);
        }
    }
}
