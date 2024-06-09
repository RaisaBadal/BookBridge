using BookBridge.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookBridge.Application.Models.Request
{
    public class BorrowRecordModel
    {
        [DataType(DataType.Date)]
        public DateTime BorrowDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReturnDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public long BookId { get; set; }

        public string UserId { get; set; }

    }
}
