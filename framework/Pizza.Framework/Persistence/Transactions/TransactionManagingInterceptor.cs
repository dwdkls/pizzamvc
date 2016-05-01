using Castle.DynamicProxy;
using NHibernate;
using NLog;
using Pizza.Utils;
using System;
using Pizza.Framework.Persistence.Transactions.Utils;

namespace Pizza.Framework.Persistence.Transactions
{
    public class TransactionManagingInterceptor : Castle.DynamicProxy.IInterceptor
    {
        // TODO: remove logger or move to client code
        private readonly Logger logger;
        private readonly LoggingHelper loggingHelper;

        private readonly ISession session;

        private ITransaction transaction;

        public TransactionManagingInterceptor(ISession session)
        {
            this.session = session;
            this.logger = LogManager.GetCurrentClassLogger();
            this.loggingHelper = new LoggingHelper(this.logger);
        }

        public void Intercept(IInvocation invocation)
        {
            if (this.IsTransactional(invocation))
            {
                this.loggingHelper.LogTransactionMethodBegin(invocation);

                bool iAmTheFirst = default(bool);
                try
                {
                    iAmTheFirst = this.BeginTransactionIfNeeded();
                    invocation.Proceed();
                    this.TryCommit(iAmTheFirst, invocation);
                }
                catch (Exception ex)
                {
                    this.logger.Error(ex, "Exception in method: {0}", invocation.Method.GetFullMethodName());
                    this.RollbackTransaction(iAmTheFirst, invocation);

                    if (InvocationHelper.IsKnownCrudOperation(invocation))
                    {
                        invocation.ReturnValue = InvocationHelper.BuildReturnValueForError(invocation, ex);
                    }
                    else
                    {
                        throw;
                    }
                }
                finally
                {
                    this.session.Close();
                }
            }
            else
            {
                this.loggingHelper.LogTransactionMethodIgnored(invocation);
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

        private void TryCommit(bool iAmTheFirst, IInvocation invocation)
        {
            if (iAmTheFirst)
            {
                this.logger.Trace("Commit transaction for method: {0}", invocation.Method.GetFullMethodName());

                this.transaction.Commit();
                this.transaction = null;
            }
        }

        private void RollbackTransaction(bool iAmTheFirst, IInvocation invocation)
        {
            if (iAmTheFirst)
            {
                this.logger.Trace("Rollback transaction for method: {0}", invocation.Method.GetFullMethodName());

                this.transaction.Rollback();
                this.session.Clear();
                this.transaction = null;
            }
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
