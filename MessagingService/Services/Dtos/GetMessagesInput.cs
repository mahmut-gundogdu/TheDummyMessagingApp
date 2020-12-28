using System.ComponentModel.DataAnnotations;

namespace MessagingService.Services.Dtos
{
    public class GetMessagesInput
    {
        [Required]
        public string ReceiptUserName { get; set; }
        
        [Required]
        public int Skip { get; set; }
        
        [Required]
        public int Take { get; set; }
    }
}