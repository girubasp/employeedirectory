using System.Collections.Generic;
using NHibernate;

namespace DataAccess.Core
{
    public interface IQuery<T>
    {
        IList<T> Execute(ISession session);
    }
    public interface IPagedQuery<T>
    {
        PagedResult<T> Execute(ISession session);
    }
}
