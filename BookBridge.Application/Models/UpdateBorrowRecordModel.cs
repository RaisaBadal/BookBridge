using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBridge.Application.Models
{
    public class UpdateBorrowRecordModel
    {
        public long bookId { get; set; }
        public string userId { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
    }
}
