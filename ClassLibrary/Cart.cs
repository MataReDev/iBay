using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary
{
    [Table("Cart")]
    public class Cart
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public bool IsValidated { get; set; }
        public virtual ICollection<Product> ListOfProducts { get; set; }
        public DateTime DateValidation { get; set; }

        public Cart()
        {
            this.ListOfProducts = new List<Product>();
        }
    }

}