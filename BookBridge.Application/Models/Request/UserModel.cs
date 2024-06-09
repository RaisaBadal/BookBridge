using System.ComponentModel.DataAnnotations;

namespace BookBridge.Application.Models.Request
{
    public class UserModel
    {
        [RegularExpression(@"^[\p{L}\p{M}' \-]{2,50}$", ErrorMessage = "Invalid name format.")]
        [StringLength(50, ErrorMessage = "this name is not allowed", MinimumLength = 2)]
        public required string Name { get; set; }

        [RegularExpression(@"^[\p{L}\p{M}' \-]{4,100}$", ErrorMessage = "Invalid name format.")]
        [StringLength(100, ErrorMessage = "this surname is not allowed", MinimumLength = 4)]
        public required string Surname { get; set; }

        [StringLength(11, ErrorMessage = "Personal number must be exactly 11 digit", MinimumLength = 11)]
        public required string PersonalNumber { get; set; }

        public required string Email { get; set; }

        public required string Password { get; set; }

        public required string ConfirmedPassword { get; set; }

        public required string UserName { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
    }
}
