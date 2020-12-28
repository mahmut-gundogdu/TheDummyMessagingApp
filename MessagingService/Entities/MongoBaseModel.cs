using MongoDB.Bson;

namespace MessagingService.Entities
{
    public class MongoBaseModel
    {
        public ObjectId Id { get; set; }
    }
}