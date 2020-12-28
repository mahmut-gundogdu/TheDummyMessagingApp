using System.ComponentModel.DataAnnotations;

namespace MessagingService.Controllers.Dtos
{
    public class UnblockUserInput
    {
        [Required]
        public string UnblockedUserName { get; set; }
    }
}