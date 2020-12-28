using System;
using System.Collections.Generic;
 

namespace MessagingService.Entities
{
    public class ConversationModel : MongoBaseModel
    {
        public List<string> Users { get; set; } = new List<string>();
        public List<MessageModel> Messages { get; set; } = new List<MessageModel>();
        public DateTime LastMessageDateTime { get; set; }
    }
}