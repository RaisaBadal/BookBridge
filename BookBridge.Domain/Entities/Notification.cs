using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookBridge.Domain.Entities
{
    [Table("Notifications")]
    [Index(nameof(CreatedDate))]
    public class Notification: AbstractClass
    {
        [StringLength(500,ErrorMessage = "This message is not valid",MinimumLength = 5)]
        public required string Message { get; set; }

        public DateTime CreatedDate { get; set; }

        public ICollection<UserNotification> UserNotifications { get; set; }
    }
}
