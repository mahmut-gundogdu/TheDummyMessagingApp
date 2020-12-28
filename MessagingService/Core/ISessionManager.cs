using MessagingService.Models.Users;

namespace MessagingService.Core
{
    public  interface ISessionManager
    {
        string GetCurrentUserName();
        User GetCurrentUser();
    }
}