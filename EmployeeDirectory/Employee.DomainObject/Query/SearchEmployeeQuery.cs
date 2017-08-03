using System;
using System.Collections;
using System.Linq;
using DataAccess.Core;
using Employee.Common;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace Employee.DomainObject.Query
{
    public class SearchEmployeeQuery : IPagedQuery<Employee>
    {
        private readonly SearchBy _searchBy;
        private readonly string _searchTerm;
        private readonly int _page;
        private readonly int _pageSize;

        public SearchEmployeeQuery(SearchBy searchBy, string searchTerm, int page, int pageSize)
        {
            _searchBy = searchBy;
            _searchTerm = searchTerm;
            _page = page;
            _pageSize = pageSize;
        }
        public PagedResult<Employee> Execute(ISession session)
        {
            PhoneNumber phone = null;
            var criteria =  session
                .CreateCriteria<Employee>()
                .CreateAlias("PhoneNumbers", "phone",JoinType.LeftOuterJoin);
           
            switch (_searchBy)
            {
                case SearchBy.All:
                    var disjunction = Restrictions.Disjunction();
                    disjunction.Add(Restrictions.InsensitiveLike("Name", _searchTerm, MatchMode.Start));
                    disjunction.Add(Restrictions.InsensitiveLike("JobTitle", _searchTerm, MatchMode.Start));
                    disjunction.Add(Restrictions.InsensitiveLike("Location", _searchTerm, MatchMode.Start));
                    disjunction.Add(Restrictions.InsensitiveLike("phone.Number", _searchTerm, MatchMode.Start));
                    disjunction.Add(Restrictions.InsensitiveLike("Email", _searchTerm, MatchMode.Start));
                    criteria.Add(disjunction);
                    break;
                case SearchBy.Name:
                    criteria.Add(Restrictions.InsensitiveLike("Name", _searchTerm, MatchMode.Start));
                    break;
                case SearchBy.JobTitle:
                    criteria.Add(Restrictions.InsensitiveLike("JobTitle", _searchTerm, MatchMode.Start));
                    break;
                case SearchBy.Location:
                    criteria.Add(Restrictions.InsensitiveLike("Location", _searchTerm, MatchMode.Start));
                    break;
                case SearchBy.PhoneNumber:
                    criteria.Add(Restrictions.InsensitiveLike("phone.Number", _searchTerm, MatchMode.Start));
                    break;
                case SearchBy.Email:
                    criteria.Add(Restrictions.InsensitiveLike("Email", _searchTerm, MatchMode.Start));
                    break;
            }
            return GetPagedResultForQuery(session ,criteria, _page, _pageSize);
        }
        private PagedResult<Employee> GetPagedResultForQuery(ISession session, ICriteria criteria, int page, int pageSize)
        {
            var countCriteria = ((ICriteria)criteria.Clone()).SetProjection(Projections.CountDistinct<Employee>(x => x.Id));
            criteria.SetResultTransformer(new DistinctRootEntityResultTransformer()).SetMaxResults(pageSize).SetFirstResult((page - 1) * pageSize);
            criteria.AddOrder(Order.Asc("Name"));
             var multi = session.CreateMultiCriteria()
                .Add(countCriteria)
                .Add(criteria)
                .List();

            var result = new PagedResult<Employee>();
            result.CurrentPage = page;
            result.PageSize = pageSize;
            result.RowCount = (int)((IList)multi[0])[0];
            var pageCount = (double)result.RowCount / result.PageSize;
            result.PageCount = (int)Math.Ceiling(pageCount);

            result.Results = ((IList)multi[1]).Cast<Employee>().ToList();
            return result;
        }
    }
}
