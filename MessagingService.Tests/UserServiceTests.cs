using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using MessagingService.Core;
using MessagingService.Entities;
using MessagingService.Helpers;
using MessagingService.Models.Users;
using MessagingService.Services;
using MessagingService.Services.Dtos;
using NSubstitute;
using NSubstitute.ClearExtensions;
using Shouldly;
using Xunit;

namespace MessagingService.Tests
{
    public class UserServiceTests 
    {
        private readonly IUserService _userService;
        private readonly IMapper _mockMapper;
        private readonly IRepository<UserModel> _mockUserRepository;
        
        private UserModel _userModel = new UserModel();
        private User _user = new User();
        private readonly ISessionManager _mockSessionManager;


        public UserServiceTests()
        {
            _mockMapper = Substitute.For<IMapper>();
            _mockMapper.Map<UserModel>(Arg.Any<RegisterInput>()).Returns(_userModel);
            _mockMapper.Map<User>(Arg.Any<UserModel>()).Returns(_user);

            _mockUserRepository = Substitute.For<IRepository<UserModel>>();
            _mockSessionManager = Substitute.For<ISessionManager>();
            _mockSessionManager.GetCurrentUser().Returns(_user);
            
            _mockUserRepository.ClearSubstitute();
            _mockMapper.ClearSubstitute();

            _userService = new UserService(_mockUserRepository, _mockMapper, _mockSessionManager);
        }

        [Fact]
        public void Register_ThrowException_ExistUser()
        {
            _mockUserRepository.Get(Arg.Any<Expression<Func<UserModel, bool>>>()).Returns(new UserModel());
            //Act && Assert
            _userService.Register(new RegisterInput()).ShouldThrow<UserFriendlyException>();
        }

        [Fact]
        public async Task Register_Ok_NotExistUser()
        {
            // Arrange
            var input = new RegisterInput();
            this._userModel = null;
            //Act 
            await _userService.Register(input);
            // Assert
            _mockMapper.Received().Map<UserModel>(input);
            await _mockUserRepository.ReceivedWithAnyArgs().Create(null);
            await _mockUserRepository.ReceivedWithAnyArgs().Get(null);
        }

        [Fact]
        public async Task IsExits_False_UserNotExits()
        {
            // Arrange
            var input = "aUserName";
            _mockUserRepository.Get(Arg.Any<Expression<Func<UserModel, bool>>>()).Returns((UserModel) null);

            //Act
            var isUserExists = await _userService.IsUserExists(input);

            // Assert
            isUserExists.ShouldBe(false);
        }

        [Fact]
        public async Task IsExits_True_UserExits()
        {
            // Arrange
            var input = "aUserName";
            _mockUserRepository.Get(Arg.Any<Expression<Func<UserModel, bool>>>()).Returns(new UserModel());

            //Act
            var isUserExists = await _userService.IsUserExists(input);

            // Assert
            isUserExists.ShouldBe(true);
        }

        [Fact]
        public async Task GetUser_NotNull_ExistUser()
        {
            // Arrange
            var userName = "aUserName";
            var password = "1";
            _mockMapper.Map<User>(Arg.Any<UserModel>()).Returns(_user);
            //Act
            var user = await _userService.GetUser(userName, password);

            // Assert
            user.ShouldNotBeNull();
            _mockMapper.Received().Map<User>(Arg.Any<UserModel>());
        }

        [Fact]
        public async Task GetUser_Null_NotExistUser()
        {
            this._user = null;
            // Arrange
            var userName = "aUserName";
            var password = "1";

            //Act
            var user = await _userService.GetUser(userName, password);

            // Assert
            user.ShouldBeNull();
            _mockMapper.Received().Map<User>(Arg.Any<UserModel>());
        }

        [Fact]
        public void GetCurrentUser_User_ExitsUser()
        {
            // Arrange
            _mockMapper.Map<User>(Arg.Any<UserModel>()).Returns(new User());
            
            //Act
            var user = _userService.GetCurrentUser();

            // Assert
            user.ShouldNotBeNull();
            _mockSessionManager.Received().GetCurrentUser();
        }

        [Fact]
        public async Task UnblockUser()
        {
            // Arrange
            var input = new UserModel()
            {
                BlockedUserNames = new List<string> {"a", "b"}
            };
            var unblocked = "a";
            
            _mockUserRepository.Get(Arg.Any<Expression<Func<UserModel, bool>>>()).Returns(input);
            //Act
            await _userService.UnblockUser(unblocked);

            //Assert
 
            await _mockUserRepository.Received()
                .Update(Arg.Any<string>(), Arg.Is<UserModel>(x => !x.BlockedUserNames.Contains(unblocked)));

        }
        
        [Fact]
        public async Task blockUser()
        {
            // Arrange
            var input = new UserModel()
            {
                BlockedUserNames = new List<string> {"a", "b"}
            };
            var blocked = "c";
          
            _mockUserRepository.Get(Arg.Any<Expression<Func<UserModel, bool>>>()).Returns(input);
            //Act
            await _userService.BlockUser(blocked);

            //Assert
            await _mockUserRepository.Received()
                .Update(Arg.Any<string>(), Arg.Is<UserModel>(x => x.BlockedUserNames.Contains(blocked)));

        }
    }
}