using System.ComponentModel.DataAnnotations;

namespace BookBridge.Application.Models.Request
{
    public class ReviewModel
    {
        public int Rating { get; set; }

        [StringLength(200, ErrorMessage = "this comment is not valid", MinimumLength = 2)]
        public string? Comment { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpDateTime { get; set; }

        public long BookId { get; set; }

        public string UserId { get; set; }

    }
}
