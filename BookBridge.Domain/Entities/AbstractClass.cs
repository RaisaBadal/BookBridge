using System.ComponentModel.DataAnnotations;

namespace BookBridge.Domain.Entities
{
    public abstract class AbstractClass
    {
        [Key]
        public long Id { get; set; }
    }
}
