using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using MessagingService.Controllers.Dtos;
using MessagingService.Core;
using MessagingService.Models.Users;
using MessagingService.Services;
using MessagingService.Services.Dtos;

namespace MessagingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtManager _jwtManager;


        public UsersController(IUserService userService, 
            IJwtManager jwtManager)
        {
            _userService = userService;
            _jwtManager = jwtManager;
        }

        [Route("Register")]  
        public async Task<IActionResult> Register(RegisterInput input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var isUserExits = await _userService.IsUserExists(input.UserName);
            if (isUserExits)
            {
                return BadRequest("UserName already exits"); 
            }
            
       
            await _userService.Register(input);
            return Ok();
        }

        [HttpPost]
        [Route("Authenticate")]  
        public async Task<IActionResult> Authenticate(AuthenticateInput input)
        {
            var user = await _userService.GetUser(input.UserName,input.HashedPassword);
            if (user == null)
            {
                return Unauthorized();
            }
            var expiration = DateTime.Now.AddHours(1);
            var token = _jwtManager.GenerateJwtToken(user.UserName,expiration);
            return Ok(new AuthenticateResponse()
            {
                Token = token,
                Expiration = expiration
            });
        }
        
        [HttpPost]
        [Authorize]
        [Route("BlockUser")]
        public async Task<IActionResult> BlockUser(BlockUserInput input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _userService.BlockUser(input.BlockedUserName);
            return Ok();
        }
        [HttpPost]
        [Authorize]
        [Route("UnblockUser")]
        public async Task<IActionResult> UnblockUser(UnblockUserInput input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _userService.UnblockUser(input.UnblockedUserName);
            return Ok();
        }

    }
}