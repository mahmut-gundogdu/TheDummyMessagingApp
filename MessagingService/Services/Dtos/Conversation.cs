using System;
using System.Collections.Generic;
using MessagingService.Entities;

namespace MessagingService.Services.Dtos
{
    public class Conversation
    {
        public List<string> Users { get; set; } 
        public List<Message> Messages { get; set; }
        public DateTime LastMessageDateTime { get; set; }
    }
}