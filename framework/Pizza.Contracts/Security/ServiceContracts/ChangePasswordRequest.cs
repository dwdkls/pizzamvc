namespace Pizza.Contracts.Security.ServiceContracts
{
    public class ChangePasswordRequest
    {
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public ChangePasswordRequest(string userName, string oldPassword, string newPassword)
        {
            this.UserName = userName;
            this.OldPassword = oldPassword;
            this.NewPassword = newPassword;
        }
    }
}