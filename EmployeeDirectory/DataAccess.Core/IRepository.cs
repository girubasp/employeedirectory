using System.Collections.Generic;
using NHibernate;

namespace DataAccess.Core
{
    public interface IRepository
    {
        ISession Session { get; }

        T GetById<T, TId>(TId id);

        void Delete<T>(T entity);

        T Insert<T>(T entity);

        T Update<T>(T entity);

        IList<T> Find<T>(IQuery<T> query);
        PagedResult<T> Find<T>(IPagedQuery<T> query);
    }
}
