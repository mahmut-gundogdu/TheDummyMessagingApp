using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MessagingService.Core;
using MessagingService.Services;
using MessagingService.Services.Dtos;
using Microsoft.Extensions.Logging;

namespace MessagingService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagingController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly ILogger<MessagingController> _logger;

        public MessagingController(IMessageService  messageService,ILogger<MessagingController> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post(SendMessageInput input)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _logger.LogInformation("Mesaj gonderilecek");
            await _messageService.SendMessage(input);
            _logger.LogInformation("Mesaj Gonderdildi.");

            return Ok();
        }

        [HttpGet]
        [Route("{userName}")]
        public async Task<IActionResult> Get(string userName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _logger.LogInformation($"{userName} e ait mesajlar goruntulenmek istendi");
            var conversations = await _messageService.GetConversation(userName);
            _logger.LogInformation($"{userName} e ait mesajlar goruntulendi.{conversations.Messages.Count} adet mesaj mevcut");
            return Ok(conversations);
        }
        [HttpGet]
        public async Task<IActionResult> Conversations(int take = 10, int skip = 0)
        {
            _logger.LogInformation($"Tum mesajlar goruntulenmek istendi");
            var conversations = await _messageService.GetConversations(take, skip);
            _logger.LogInformation($"Tum mesajlar goruntulendi.{conversations.Count} adet konusma mevcut");
            return Ok(conversations);
        }
    }
}