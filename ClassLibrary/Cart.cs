using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary
{
    [Table("Cart")]
    public class Cart
    {
        public int Id { get; set; }
        public virtual User user { get; set; }
        public bool isValidated { get; set; }
        public virtual ICollection<Product> listOfProducts { get; set; }
        public DateTime dateValidation { get; set; }

        public Cart()
        {
            listOfProducts = new List<Product>();
        }

    }

}