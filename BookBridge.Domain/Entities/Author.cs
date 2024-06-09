using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookBridge.Domain.Entities
{
    [Table("Authors")]
    [Index(nameof(BirthDate))]
    public class Author: AbstractClass
    {
        [Column("Name")]
        [StringLength(50, ErrorMessage = "this name is not allowed", MinimumLength = 2)]
        public required string Name { get; set; }

        [Column("Surname")]
        [StringLength(50, ErrorMessage = "this surname is not allowed", MinimumLength = 2)]
        public required string Surname { get; set; }

        [DataType(DataType.Date)]
        public required DateTime BirthDate { get; set; }

        public virtual IEnumerable<Book>Books { get; set; }

        public bool IsActive { get; set; } = true;


    }
}
