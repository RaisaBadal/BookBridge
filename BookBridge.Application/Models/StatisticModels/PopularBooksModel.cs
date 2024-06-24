using System.ComponentModel.DataAnnotations;

namespace BookBridge.Application.Models.StatisticModels
{
    public class PopularBooksModel
    {
        public long Id { get; set; }

        [StringLength(50, ErrorMessage = "such book title is not valid", MinimumLength = 2)]
        public required string Title { get; set; }

        public required int count { get; set; }
    }
}
