using System;
using NHibernate;

namespace Pizza.Framework.Persistence.Exceptions
{
    public class OptimisticConcurrencyException : Exception
    {
        public OptimisticConcurrencyException(string message, StaleObjectStateException sosex)
            : base(message, sosex)
        {
        }
    }
}
