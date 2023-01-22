using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary
{
    [Table("Cart")]
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime DateValidation { get; set; }
    }

}