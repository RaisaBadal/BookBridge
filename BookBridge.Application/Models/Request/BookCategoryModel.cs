using System.ComponentModel.DataAnnotations;

namespace BookBridge.Application.Models.Request
{
    public class BookCategoryModel
    {
        public long Id { get; set; }

        [StringLength(50, ErrorMessage = "this category name is not valid", MinimumLength = 2)]
        public required string Name { get; set; }

        [StringLength(50, ErrorMessage = "this category description  is not valid", MinimumLength = 2)]
        public required string Description { get; set; }

    }
}
