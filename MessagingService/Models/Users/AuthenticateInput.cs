namespace MessagingService.Models.Users
{
    public class AuthenticateInput
    {
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
    }
}