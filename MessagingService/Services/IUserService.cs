using System.Threading.Tasks;
using MessagingService.Models.Users;
using MessagingService.Services.Dtos;

namespace MessagingService.Services
{
    public interface IUserService
    {
        Task Register(RegisterInput user);
        Task<User> GetUser(string userName, string password);
        Task<User> GetUser(string userName);
        Task<bool> IsUserExists(string userName);
        User GetCurrentUser();
        Task UnblockUser(string unblockedUserName);
        Task BlockUser(string blockedUserName);
        Task<bool> IsUserBlocked(string receiverUserName);
    }
}