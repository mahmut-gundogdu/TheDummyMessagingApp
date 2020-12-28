using System.ComponentModel.DataAnnotations;

namespace MessagingService.Services.Dtos
{
    public class SendMessageInput
    {
        [Required]
        public string ReceiverUserName{ get; set; }
        [Required]
        public string Message { get; set; }
    }
}