using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary
{
    [Table("ProductCart")]
    public class ProductCart
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
    }
}
