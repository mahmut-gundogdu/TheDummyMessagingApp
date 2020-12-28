using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagingService.Helpers;
using MessagingService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MessagingService.Core
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IJwtManager _jwtManager;
    
        
        public JwtMiddleware(RequestDelegate next,  IJwtManager jwtManager)
        {
            _next = next;
            _jwtManager = jwtManager;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
               await AttachUserToContext(context, userService, token);

            await _next(context);
        }

        private async Task AttachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                var userName = _jwtManager.ValidateAndGetUserName(token);
                context.Items["User"] = await  userService.GetUser(userName);
            }
            catch
            {
             //   _logger.LogInformation("Failed login attempt");
            }
        }
        
    }
}