using System;

namespace Pizza.Framework.Persistence.Transactions
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class TransactionalAttribute : Attribute
    {
    }
}