using System.ComponentModel.DataAnnotations;

namespace BookBridge.Application.Models.Request
{
    public class NotificationModel
    {
        [StringLength(500, ErrorMessage = "This message is not valid", MinimumLength = 5)]
        public required string Message { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? SentDate { get; set; }

    }
}
