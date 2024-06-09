using System.ComponentModel.DataAnnotations.Schema;

namespace BookBridge.Domain.Entities
{
    [Table("Wishlists")]
    public class Wishlist: AbstractClass
    {
        public DateTime? AddedDate { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }

        public IEnumerable<WishlistItem> WishlistItems { get; set; }
    }
}
