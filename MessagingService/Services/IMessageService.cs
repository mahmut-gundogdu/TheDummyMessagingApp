using System.Collections.Generic;
using System.Threading.Tasks;
using MessagingService.Services.Dtos;

namespace MessagingService.Services
{
    public interface IMessageService
    {
        Task SendMessage(SendMessageInput input);
        Task<IList<Message>> GetMessages(GetMessagesInput input);
        Task<IList<Conversation>> GetConversations(int take, int skip);
        Task<Conversation> GetConversation(string userName);
    }
}