using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
        public float price { get; set; }
        public bool available { get; set; }
        public DateTime added_time { get; set; }

    }
}