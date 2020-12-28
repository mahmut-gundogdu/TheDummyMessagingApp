using AutoMapper;
using MessagingService.Entities;
using MessagingService.Services.Dtos;

namespace MessagingService.Core.Profiles
{
    public class MessageProfile: Profile
    {
        public MessageProfile()
        {
            CreateMap<MessageModel, Message>();
            CreateMap<ConversationModel, Conversation>();
        }
    }
}