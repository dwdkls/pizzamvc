using System;
using System.Text;
using Castle.DynamicProxy;
using NHibernate;
using NLog;
using Pizza.Framework.Persistence.Exceptions;
using Pizza.Utils;

namespace Pizza.Framework.Persistence.Transactions
{
    public class TransactionManagingInterceptor : Castle.DynamicProxy.IInterceptor
    {
        // TODO: remove logger or move to client code
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ISession session;

        private ITransaction transaction;

        public TransactionManagingInterceptor(ISession session)
        {
            this.session = session;
        }

        public void Intercept(IInvocation invocation)
        {
            string methodName = string.Format("{0}.{1}", invocation.Method.DeclaringType.Name, invocation.Method.Name);
            var argumentsList = BuildArgumentsList(invocation);
            bool isTransactional = this.IsTransactional(invocation);

            if (isTransactional)
            {
                this.LogTransactionMethodBegin(methodName, argumentsList);

                var iAmTheFirst = this.BeginTransactionIfNeeded();

                try
                {
                    invocation.Proceed();
                    this.TryCommit(iAmTheFirst, methodName);
                }
                catch (StaleObjectStateException sosex)
                {
                    this.RollbackTransaction(iAmTheFirst, methodName, sosex);
                    throw new OptimisticConcurrencyException("This record has been updated by someone else.", sosex);
                }
                catch (Exception ex)
                {
                    this.RollbackTransaction(iAmTheFirst, methodName, ex);
                    throw;
                }
                finally
                {
                    this.session.Close();
                }
            }
            else
            {
                this.LogTransactionMethodIgnored(methodName, argumentsList);
                invocation.Proceed();
            }
        }

        private bool BeginTransactionIfNeeded()
        {
            if (this.transaction == null)
            {
                this.transaction = this.session.BeginTransaction();
                return true;
            }

            return false;
        }

        private void TryCommit(bool iAmTheFirst, string methodName)
        {
            if (iAmTheFirst)
            {
                this.logger.Trace("Commit transaction for method: {0}", methodName);

                this.transaction.Commit();
                this.transaction = null;
            }
        }

        private void RollbackTransaction(bool iAmTheFirst, string methodName, Exception ex)
        {
            if (iAmTheFirst)
            {
                this.logger.Trace("Rollback transaction for method: {0}", methodName);

                this.transaction.Rollback();
                this.session.Clear();
                this.transaction = null;
            }

            this.logger.ErrorException(string.Format("Error commiting transaction for method: {0}", methodName), ex);
        }


        private static string BuildArgumentsList(IInvocation invocation)
        {
            var argumentsList = new StringBuilder("Arguments: " + Environment.NewLine);
            for (int i = 0; i < invocation.Arguments.Length; i++)
            {
                argumentsList.AppendLine(string.Format("Id: {0}\tValue: {1}", i, invocation.Arguments[i]));
            }

            return argumentsList.ToString();
        }

        private void LogTransactionMethodBegin(string methodName, string argumentsList)
        {
            this.logger.Trace("TransactionManagingInterceptor executes method: {0}", methodName);
            this.logger.Trace(argumentsList);
        }

        private void LogTransactionMethodIgnored(string methodName, string argumentsList)
        {
            this.logger.Trace("TransactionManagingInterceptor ignores method: {0}", methodName);
            this.logger.Trace(argumentsList);
        }


        private bool IsTransactional(IInvocation invocation)
        {
            //var classAttribute = methodInfo.DeclaringType.GetAttribute<TransactionalAttribute>();
            var classAttribute = invocation.TargetType.GetAttribute<TransactionalAttribute>();
            var methodAttribute = invocation.Method.GetAttribute<TransactionalAttribute>();

            // properties are ignored
            return (classAttribute != null || methodAttribute != null) && !invocation.Method.IsSpecialName;
        }
    }
}
