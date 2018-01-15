namespace Pizza.Contracts.Security.ServiceContracts
{
    public class ChangePasswordResult
    {
        public bool Succeed
        {
            get { return string.IsNullOrEmpty(this.ErrorMessage); }
        }

        public string ErrorMessage { get; }

        public ChangePasswordResult()
        {
        }

        public ChangePasswordResult(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }
    }
}