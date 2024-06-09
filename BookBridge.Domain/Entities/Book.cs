using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookBridge.Domain.Entities
{
    [Table("Books")]
    public class Book: AbstractClass
    {
        [StringLength(50,ErrorMessage = "such book title is not valid",MinimumLength = 2)]
        public required string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }

        [StringLength(500, ErrorMessage = "such book title is not valid", MinimumLength = 2)]
        public required string Description { get; set; }

        public required string CoverImageUrl { get; set; }

        public int AvailableCopies { get; set; }

        public int TotalCopies { get; set; }

        [ForeignKey(nameof(Authors))]
        public required long AuthorId { get; set; }

        public virtual IEnumerable<Author> Authors { get; set; }

        [ForeignKey(nameof(BookCategories))]
        public required long BookCategoryId { get; set; }

        public IEnumerable<BookCategory>BookCategories { get;set; }

        public IEnumerable<BorrowRecord>BorrowRecords { get; set; }

        public IEnumerable<Review>Reviews { get;set; }

        public IEnumerable<WishlistItem>WishlistItems { get;set; }

        public bool IsActive { get; set; } = true;
    }
}
