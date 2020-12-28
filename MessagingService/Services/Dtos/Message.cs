using System;

namespace MessagingService.Services.Dtos
{
    public class Message
    {
        public string SenderUserName { get; set; }
        public string ReceiverUserName { get; set; }
        public string MessageText { get; set; }
        public DateTime CreateAt { get; set; }
    }
}