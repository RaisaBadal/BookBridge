using System.ComponentModel.DataAnnotations.Schema;

namespace BookBridge.Domain.Entities
{
    [Table("UserNotifications")]
    public class UserNotification:AbstractClass
    {
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(Notification))]
        public long NotificationId { get; set; }
        public Notification Notification { get; set; }

        public DateTime? SentDate { get; set; }

        public bool IsSent { get; set; } = false;
    }
}
