using System;
using System.Data.SqlClient;
using Castle.DynamicProxy;
using NHibernate;
using Pizza.Contracts.Operations.Results;

namespace Pizza.Framework.Persistence.Transactions.Utils
{
    internal static class InvocationHelper
    {
        public static bool IsKnownCrudOperation(IInvocation invocation)
        {
            var returnType = invocation.Method.ReturnType;
            return returnType.BaseType != null && returnType.BaseType == typeof(CrudOperationResultBase);
        }

        public static object BuildReturnValueForError(IInvocation invocation, Exception ex)
        {
            var returnType = invocation.Method.ReturnType;

            var resultTypeToCreate = returnType;
            if (returnType.IsGenericType)
            {
                var genericTypeDefinition = returnType.GetGenericTypeDefinition();
                var genericArguments = returnType.GetGenericArguments();
                resultTypeToCreate = genericTypeDefinition.MakeGenericType(genericArguments);
            }

            var state = GetCrudOperationStateForException(ex);

            object crudOperationResult = Activator.CreateInstance(resultTypeToCreate, state, ex.Message);
            return crudOperationResult;
        }

        private static CrudOperationState GetCrudOperationStateForException(Exception ex)
        {
            var state = CrudOperationState.OtherError;
            if (ex is StaleObjectStateException)
            {
                state = CrudOperationState.OptimisticConcurrencyError;
            }
            else if (ex is SqlException)
            {
                state = CrudOperationState.DatabaseError;
            }
            return state;
        }
    }
}