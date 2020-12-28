using System.Threading.Tasks;
using MessagingService.Helpers;
using MessagingService.Models.Users;
using Microsoft.AspNetCore.Http;

namespace MessagingService.Core
{
    public class SessionManager:ISessionManager
    {
        private readonly HttpContext? _httpContext;

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }
        public string GetCurrentUserName()
        {
            var user  = GetCurrentUser();
            if (user  == null)
            {
                throw new UserFriendlyException("the User unauthorized");
            }
            return user.UserName;
        }

        public User GetCurrentUser()
        {
            return (User) _httpContext?.Items["User"];
        }
    }
}