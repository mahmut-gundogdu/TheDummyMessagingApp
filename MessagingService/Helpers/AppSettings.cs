using System.Security.Principal;

namespace MessagingService.Helpers
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public JwtSettings Jwt { get; set; }
    }

    public class ConnectionStrings
    {
        public string Default { get; set; }
        public string DatabaseName { get; set; }
    }

    public class JwtSettings
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string Secret { get; set; }
    }
}