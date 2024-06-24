namespace BookBridge.Application.Models.Request
{
    public class UserNotificationModel
    {
        public string UserId { get; set; }

        public long NotificationId { get; set; }

        public DateTime? SentDate { get; set; }

        public bool IsSent { get; set; } = false;
    }
}
