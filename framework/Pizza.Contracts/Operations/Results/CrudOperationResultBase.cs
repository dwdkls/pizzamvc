namespace Pizza.Contracts.Operations.Results
{
    public abstract class CrudOperationResultBase
    {
        public CrudOperationState State { get; }
        public string ErrorMessage { get; }

        public bool Succeed
        {
            get { return this.State == CrudOperationState.Success; }
        }

        protected CrudOperationResultBase(CrudOperationState state, string errorMessage)
        {
            this.State = state;
            this.ErrorMessage = errorMessage;
        }
    }
}