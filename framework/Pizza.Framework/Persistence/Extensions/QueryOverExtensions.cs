using System;
using NHibernate;

namespace Pizza.Framework.Persistence.Extensions
{
    public static class QueryOverExtensions
    {
        public static IQueryOver<TPersistenceModel> ApplyPaging<TPersistenceModel>(
            this IQueryOver<TPersistenceModel> queryOver, int currentPageNumber, int pageSize)
        {
            if (currentPageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("currentPageNumber", "Page number can't be less than 1");
            }

            var query = queryOver.Skip((currentPageNumber - 1) * pageSize)
                .Take(pageSize);

            return query;
        }
    }
}