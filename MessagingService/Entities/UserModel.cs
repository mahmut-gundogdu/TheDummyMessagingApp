using System.Collections.Generic;
using MessagingService.Services;

namespace MessagingService.Entities
{
    public class UserModel: MongoBaseModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<string> BlockedUserNames { get; set; } = new List<string>();
    }
}