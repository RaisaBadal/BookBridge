using System.ComponentModel.DataAnnotations;

namespace BookBridge.Application.Models
{
    public class DateModel
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
    }
}
