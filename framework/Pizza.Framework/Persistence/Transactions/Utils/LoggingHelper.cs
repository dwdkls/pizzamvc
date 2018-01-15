using System;
using System.Text;
using Castle.DynamicProxy;
using NLog;

namespace Pizza.Framework.Persistence.Transactions.Utils
{
    internal class LoggingHelper
    {
        private readonly Logger logger;

        public LoggingHelper(Logger logger)
        {
            this.logger = logger;
        }

        public void LogTransactionMethodBegin(IInvocation invocation)
        {
            string methodName = $"{invocation.Method.DeclaringType.Name}.{invocation.Method.Name}";
            var argumentsList = BuildArgumentsList(invocation);
            this.logger.Trace("TransactionManagingInterceptor executes method: {0}", methodName);
            this.logger.Trace(argumentsList);
        }

        public void LogTransactionMethodIgnored(IInvocation invocation)
        {
            string methodName = $"{invocation.Method.DeclaringType.Name}.{invocation.Method.Name}";
            var argumentsList = BuildArgumentsList(invocation);
            this.logger.Trace("TransactionManagingInterceptor ignores method: {0}", methodName);
            this.logger.Trace(argumentsList);
        }

        private static string BuildArgumentsList(IInvocation invocation)
        {
            var argumentsList = new StringBuilder("Arguments: " + Environment.NewLine);
            for (int i = 0; i < invocation.Arguments.Length; i++)
            {
                argumentsList.AppendFormat("Id: {0}\tValue: {1}", i, invocation.Arguments[i]).AppendLine();
            }

            return argumentsList.ToString();
        }
    }
}