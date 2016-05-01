namespace Pizza.Contracts.Operations.Results
{
    public enum CrudOperationState
    {
        Undefined,
        Success,
        DatabaseError,
        AccessDeniedError,
        OtherError,
        OptimisticConcurrencyError
    }
}