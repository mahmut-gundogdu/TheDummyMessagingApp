using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using MessagingService.Core;
using MessagingService.Entities;
using MessagingService.Helpers;
using MessagingService.Models.Users;
using MessagingService.Services.Dtos;

namespace MessagingService.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<UserModel> _userRepository;
        private readonly IMapper _mapper;
        private readonly ISessionManager _sessionManager;
        public UserService(IRepository<UserModel> userRepository,
            IMapper mapper, 
            ISessionManager sessionManager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _sessionManager = sessionManager;
        }

        public async Task Register(RegisterInput input)
        {
            var entity = _mapper.Map<UserModel>(input);

            var isUserExits = await IsUserExists(input.UserName);
            if (isUserExits)
            {
                throw new UserFriendlyException("User already exists");
            }

            await _userRepository.Create(entity);
        }
        public async Task<bool> IsUserExists(string userName)
        {
            var user = await _userRepository.Get(x => x.UserName == userName);
            return user != null;
        }
        public async Task<User> GetUser(string userName, string hashedPassword)
        {
            var entity = await _userRepository.Get(x => x.UserName == userName && x.Password == hashedPassword);
            return _mapper.Map<User>(entity);
        }
        public async Task<User> GetUser(string userName)
        {
            var entity = await _userRepository.Get(x => x.UserName == userName);
            return _mapper.Map<User>(entity);
        }

        private string GetUserName()
        {
            return _sessionManager.GetCurrentUserName();
        }

        public User GetCurrentUser()
        {
            return _sessionManager.GetCurrentUser();
        }

        public async Task BlockUser(string blockedUserName)
        {
            var currentUserName = GetUserName();

            var userModel = await _userRepository.Get(x => x.UserName == currentUserName);
            userModel.BlockedUserNames.Add(blockedUserName);
            await _userRepository.Update(userModel.Id.ToString(), userModel);
        }
        public async Task UnblockUser(string unblockedUserName)
        {
            var currentUserName = GetUserName();
            var userModel = await _userRepository.Get(x => x.UserName == currentUserName);
            userModel.BlockedUserNames.Remove(unblockedUserName);
            await _userRepository.Update(userModel.Id.ToString(), userModel);
        }

        public async Task<bool> IsUserBlocked(string receiverUserName)
        {
            var currentUserName = GetUserName();
            var receiverUser = await _userRepository.Get(x => x.UserName == receiverUserName);
            return receiverUser != null && receiverUser.BlockedUserNames.Contains(currentUserName);
        }
    }
}