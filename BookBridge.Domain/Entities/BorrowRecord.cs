using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookBridge.Domain.Entities
{
    [Table("BorrowRecords")]
    [Index(nameof(BorrowDate))]
    public class BorrowRecord: AbstractClass
    {
        [DataType(DataType.Date)]
        public DateTime BorrowDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public bool IsReturned { get; set; } = false;

        [ForeignKey(nameof(Books))]
        public long BookId { get;set; }

        public IEnumerable<Book>Books { get;set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public User User { get; set; }

    }
}
