using System.ComponentModel.DataAnnotations;

namespace MessagingService.Controllers.Dtos
{
    public class BlockUserInput
    {
        [Required]
        public string BlockedUserName { get; set; }
    }
}