using System.Collections.Generic;
using NHibernate;

namespace Pizza.Framework.Persistence.Extensions
{
    public static class SessionExtensions
    {
        public static IEnumerable<TItem> SaveMany<TItem>(this ISession session, IEnumerable<TItem> items)
        {
            foreach (var item in items)
            {
                session.Save(item);
            }

            return items;
        }

        public static void FlushAndClear(this ISession session)
        {
            session.Flush();
            session.Clear();
        }
    }
}
