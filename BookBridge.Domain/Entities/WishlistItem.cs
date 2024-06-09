using System.ComponentModel.DataAnnotations.Schema;

namespace BookBridge.Domain.Entities
{
    [Table("WishlistItems")]
    public class WishlistItem: AbstractClass
    {

        [ForeignKey(nameof(Book))]
        public long BookId { get; set; }
        public Book Book { get; set; }

        [ForeignKey(nameof(Wishlist))]
        public long WishlistId { get; set; }
        public Wishlist Wishlist { get; set; }
    }
}
