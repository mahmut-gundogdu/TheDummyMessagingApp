using System;
using MessagingService.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MessagingService.Entities
{
    public class MessageModel
    {
        public MessageModel()
        {
            CreateAt = DateTime.Now;
        }
        public string SenderUserName { get; set; }
        public string ReceiverUserName { get; set; }
        public string MessageText { get; set; }
        
        [BsonRepresentation(BsonType.Document)]
        public DateTime CreateAt { get; set; }
    }
}