using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;

namespace BookBridge.Domain.Entities
{
    [Table("BookCategories")]
    public class BookCategory:AbstractClass
    {
        [Column("CategoryName")]
        [StringLength(50,ErrorMessage = "this category name is not valid",MinimumLength = 2)]
        public required string Name { get; set; }

        [Column("Description")]
        [StringLength(50, ErrorMessage = "this category description  is not valid", MinimumLength = 2)]
        public required string Description { get; set; }

        public virtual IEnumerable<Book>Books { get; set; }

        public bool IsActive { get; set; }=true;
    }
}
