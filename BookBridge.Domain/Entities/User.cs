using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookBridge.Domain.Entities
{
    [Table("Users")]
    public class User: IdentityUser
    {
        [Column("User_Name")]
        [StringLength(50, ErrorMessage = "this name is not allowed", MinimumLength = 2)]
        public required string Name { get; set; }

        [Column("User_Surname")]
        [StringLength(100, ErrorMessage = "this surname is not allowed", MinimumLength = 4)]
        public required string Surname { get; set; }

        [StringLength(11, ErrorMessage = "Personal number must be exactly 11 digit", MinimumLength = 11)]
        [Column("Personal_Number")]
        public required string PersonalNumber { get; set; }


        [Column("User_BirthDay")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual IEnumerable<BorrowRecord> BorrowRecords { get; set; }

        public virtual IEnumerable<Review> Reviews { get; set; }

        public virtual Wishlist Wishlist { get; set; }

        public virtual IEnumerable<Notification>Notifications { get; set; }
    }
}
