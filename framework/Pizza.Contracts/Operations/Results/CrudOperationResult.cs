namespace Pizza.Contracts.Operations.Results
{
    public sealed class CrudOperationResult<TResultType> : CrudOperationResultBase
    {
        public TResultType Data { get; }

        public CrudOperationResult(CrudOperationState state, string errorMessage)
            : base(state, errorMessage)
        {
        }

        public CrudOperationResult(TResultType data)
            : base(CrudOperationState.Success, null)
        {
            this.Data = data;
        }
    }

    public sealed class CrudOperationResult : CrudOperationResultBase
    {
        public CrudOperationResult(CrudOperationState state, string errorMessage)
            : base(state, errorMessage)
        {
        }

        public static CrudOperationResult Success
        {
            get { return new CrudOperationResult(CrudOperationState.Success, null); }
        }
    }
}
