namespace Pizza.Contracts.Presentation.Security.ServiceContracts
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginRequest(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
        }
    }
}