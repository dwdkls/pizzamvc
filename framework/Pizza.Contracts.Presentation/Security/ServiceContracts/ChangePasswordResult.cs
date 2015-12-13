namespace Pizza.Contracts.Presentation.Security.ServiceContracts
{
    public class ChangePasswordResult
    {
        public bool Succeed
        {
            get { return string.IsNullOrEmpty(this.ErrorMessage); }
        }

        public string ErrorMessage { get; private set; }

        public ChangePasswordResult()
        {
        }

        public ChangePasswordResult(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }
    }
}