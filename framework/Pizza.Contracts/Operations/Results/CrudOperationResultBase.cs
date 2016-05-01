namespace Pizza.Contracts.Operations.Results
{
    public abstract class CrudOperationResultBase
    {
        public CrudOperationState State { private set; get; }
        public string ErrorMessage { get; private set; }

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