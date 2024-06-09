using System.ComponentModel.DataAnnotations;

namespace BookBridge.Application.Models.Request
{
    public class BookModel
    {
        public long Id { get; set; }

        [StringLength(50, ErrorMessage = "such book title is not valid", MinimumLength = 2)]
        public required string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }

        [StringLength(500, ErrorMessage = "such book title is not valid", MinimumLength = 2)]
        public required string Description { get; set; }

        public required string CoverImageUrl { get; set; }

        public required long AuthorId { get; set; }

        public required long BookCategoryId { get; set; }

        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }

    }
}
