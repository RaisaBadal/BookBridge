using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookBridge.Domain.Entities
{
    [Table("Notifications")]
    [Index(nameof(CreatedDate))]
    [Index(nameof(SentDate))]
    public class Notification: AbstractClass
    {
        [ForeignKey(nameof(Users))]
        public string UserId { get; set; }
        public IEnumerable<User>Users { get; set; }

        [StringLength(500,ErrorMessage = "This message is not valid",MinimumLength = 5)]
        public required string Message { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? SentDate { get; set; }

        public bool IsSent { get; set; } = false;
    }
}
