using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookBridge.Domain.Entities
{
    [Table("Reviews")]
    public class Review: AbstractClass
    {
        [Column("BookRating")]
        public int Rating { get; set; }

        [StringLength(200,ErrorMessage = "this comment is not valid",MinimumLength = 2)]
        public string? Comment { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpDateTime { get; set; }

        [ForeignKey(nameof(Book))]
        public long BookId {get; set; }

        public Book Book { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public User User { get; set; }

        public bool IsActive { get; set; }=true;
    }
}
