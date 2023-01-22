using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary
{
    [Table("ProductCart")]
    public class ProductCart
    {
        public int Id { get; set; }
        public virtual Cart cart { get; set; }
        public int productId { get; set; }
    }
}
