using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using MessagingService.Core;
using MessagingService.Entities;
using MessagingService.Helpers;
using MessagingService.Models.Users;
using MessagingService.Services;
using MessagingService.Services.Dtos;
using MongoDB.Bson;
using NSubstitute;
using Shouldly;
using Xunit;

namespace MessagingService.Tests
{
    public class MessagingServiceTests
    {
        private readonly MessageService _messageService;
        private readonly IUserService _mockUserService;
        private readonly IRepository<ConversationModel> _mockRepository;
        private readonly IMapper _mockMapper;
        private User _loggedUser;

        public MessagingServiceTests()
        {
            _mockUserService = Substitute.For<IUserService>();
            _mockMapper = Substitute.For<IMapper>();
            _mockRepository = Substitute.For<IRepository<ConversationModel>>();
            _messageService = new MessageService(_mockUserService, _mockRepository, _mockMapper);
            _loggedUser = new User() {UserName = "a.b", Email = "a@b.com", Name = "John", Surname = "Doe"};
            _mockUserService.GetCurrentUser().ReturnsForAnyArgs(_loggedUser);
        }

        [Fact]
        public void SendMessage_EmptyMessage_ThrowError()
        {
            //Arrange
            var input = new SendMessageInput()
            {
                Message = string.Empty,
                ReceiverUserName = "a"
            };

            //Act && Assert
            _messageService.SendMessage(input).ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void SendMessage_EmptyReceiverUserName_ThrowError()
        {
            //Arrange
            var input = new SendMessageInput()
            {
                Message = "b",
                ReceiverUserName = string.Empty
            };

            //Act && Assert
            _messageService.SendMessage(input).ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public async Task SendMessage_NewConversation()
        {
            //Arrange
            var input = new SendMessageInput()
            {
                Message = "message",
                ReceiverUserName = "RUser"
            };
            _mockRepository.Get(Arg.Any<Expression<Func<ConversationModel, bool>>>())
                .ReturnsForAnyArgs((ConversationModel) null);

            //Act
            await _messageService.SendMessage(input);

            //Assert
            await _mockRepository.Received().Create(Arg.Is<ConversationModel>(x =>
                x.Messages.Any(m => m.MessageText == input.Message
                                    && m.ReceiverUserName == input.ReceiverUserName
                                    && m.SenderUserName == _loggedUser.UserName)));
        }

        [Fact]
        public async Task SendMessage_ExistConversation()
        {
            //Arrange
            var input = new SendMessageInput()
            {
                Message = "message",
                ReceiverUserName = "RUser"
            };
            var existsConversation = new ConversationModel() {Id = new ObjectId()};
            _mockRepository.Get(Arg.Any<Expression<Func<ConversationModel, bool>>>())
                .ReturnsForAnyArgs(existsConversation);

            //Act
            await _messageService.SendMessage(input);

            //Assert
            await _mockRepository.Received().Update(Arg.Is(existsConversation.Id.ToString()),
                Arg.Is<ConversationModel>(x =>
                    x.Messages.Any(m => m.MessageText == input.Message
                                        && m.ReceiverUserName == input.ReceiverUserName
                                        && m.SenderUserName == _loggedUser.UserName)));
        }

        [Fact]
        public void SendMessage_blockedUser_ThrowError()
        {
            //Arrange
            var input = new SendMessageInput()
            {
                Message = "abc",
                ReceiverUserName = "a"
            };
            _mockUserService.IsUserBlocked(Arg.Any<string>()).ReturnsForAnyArgs(true);
            

            //Act && Assert
            _messageService.SendMessage(input).ShouldThrow<UserFriendlyException>();
        }

        [Fact]
        public async Task GetConversations_WithoutException()
        {
            //Act
            await _messageService.GetConversations(10, 0);
            //Assert

            _mockMapper.ReceivedWithAnyArgs();
            _mockRepository.ReceivedWithAnyArgs();
        }
        
        [Fact]
        public async Task GetConversation_WithoutException()
        {
            //Act
            await _messageService.GetConversation(_loggedUser.UserName);
            //Assert

            _mockMapper.ReceivedWithAnyArgs();
            _mockRepository.ReceivedWithAnyArgs();
        }
    }
}