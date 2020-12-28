using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using MessagingService.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MessagingService.Core
{
    public class JwtManager : IJwtManager
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly JwtSettings _jwt;

        public JwtManager(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
            _jwt = _appSettings.Value.Jwt;
        }

        public string GenerateJwtToken(string userName, DateTime expires)
        {
            var jwtSettings = _appSettings.Value.Jwt;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string ValidateAndGetUserName(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwt.Secret);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken) validatedToken;
            var claim = jwtToken.Claims.FirstOrDefault(x => x.Type == "unique_name");
            return claim?.Value;
        }
    }

    public interface IJwtManager
    {
        string GenerateJwtToken(string userName,DateTime expires);
        string ValidateAndGetUserName(string token);
    }
}