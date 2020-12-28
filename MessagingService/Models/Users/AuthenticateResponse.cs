using System;

namespace MessagingService.Models.Users
{
    public class AuthenticateResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}