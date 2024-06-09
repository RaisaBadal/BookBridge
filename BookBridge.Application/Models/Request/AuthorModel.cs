using System.ComponentModel.DataAnnotations;


namespace BookBridge.Application.Models.Request
{
    public class AuthorModel
    {
        public long Id { get; set; }

        [RegularExpression(@"^[\p{L}\p{M}' \-]{1,50}$", ErrorMessage = "Invalid name format.")]
        public required string Name { get; set; }

        [RegularExpression(@"^[\p{L}\p{M}' \-]{1,50}$", ErrorMessage = "Invalid surname format.")]
        public required string Surname { get; set; }

        [DataType(DataType.Date)]
        public required DateTime BirthDate { get; set; }
    }
}
