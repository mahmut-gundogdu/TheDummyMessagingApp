using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MessagingService.Core;
using MessagingService.Entities;
using MessagingService.Helpers;
using MessagingService.Services.Dtos;
using MongoDB.Driver;


namespace MessagingService.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUserService _userService;
        private readonly IRepository<ConversationModel> _conversationRepository;
        private readonly IMapper _mapper;


        public MessageService(IUserService userService,
            IRepository<ConversationModel> conversationRepository, IMapper mapper)
        {
            _userService = userService;
            _conversationRepository = conversationRepository;
            _mapper = mapper;
        }

        public async Task SendMessage(SendMessageInput input)
        {
            if (input.Message.IsNullOrEmpty() || input.ReceiverUserName.IsNullOrEmpty())
            {
                var propertyNme = input.Message.IsNullOrEmpty()
                    ? nameof(input.Message)
                    : nameof(input.ReceiverUserName);
                throw new ArgumentNullException(propertyNme);
            }

            var user = _userService.GetCurrentUser();
            
            var isUserBlocked = await _userService.IsUserBlocked(input.ReceiverUserName);
            if (isUserBlocked)
            {
                var errorMessage = $"You can't send messaje to {input.ReceiverUserName}. You are blocked";
                throw new UserFriendlyException(errorMessage);
            }
            
            var messageModel = new MessageModel
            {
                ReceiverUserName = input.ReceiverUserName,
                MessageText = input.Message,
                SenderUserName = user.UserName
            };
            var conversation = await GetConversation(user.UserName, input.ReceiverUserName);
            if (conversation == null)
            {
                conversation = new ConversationModel();
                conversation.Messages.Add(messageModel);
                conversation.Users.Add(input.ReceiverUserName);
                conversation.Users.Add(user.UserName);
                conversation.LastMessageDateTime = messageModel.CreateAt;
                await _conversationRepository.Create(conversation);
            }
            else
            {
                conversation.Messages.Add(messageModel);
                conversation.LastMessageDateTime = messageModel.CreateAt;
                await _conversationRepository.Update(conversation.Id.ToString(), conversation);
            }
        }

        public async Task<IList<Message>> GetMessages(GetMessagesInput input)
        {
            var user = _userService.GetCurrentUser();
            var conversation = await GetConversation(user.UserName, input.ReceiptUserName);
            if (conversation == null)
            {
                return null;
            }

            var messages = conversation.Messages.SkipLast(input.Skip).Take(input.Take);
            return _mapper.Map<List<Message>>(messages);
        }


        public async Task<IList<Conversation>> GetConversations(int take, int skip)
        {
            var user = _userService.GetCurrentUser();

            var conversationsEntities = await _conversationRepository.GetAll(x => x.Users.Contains(user.UserName),
                new FindOptions<ConversationModel>()
                {
                    Sort = Builders<ConversationModel>.Sort.Descending(c => c.LastMessageDateTime),
                    Limit = take,
                    Skip = skip,
                });

            var conversationModels = await conversationsEntities.ToListAsync();
            return _mapper.Map<List<Conversation>>(conversationModels);
        }


        public async Task<Conversation> GetConversation(string userName)
        {
            var currentUser = _userService.GetCurrentUser();
            var model = await _conversationRepository.Get(x =>
                x.Users.Contains(currentUser.UserName) && x.Users.Contains(userName));
            return _mapper.Map<Conversation>(model);
        }

        private async Task<ConversationModel> GetConversation(string user1, string user2)
        {
            return await _conversationRepository.Get(x => x.Users.Contains(user1) && x.Users.Contains(user2));
        }
    }
}