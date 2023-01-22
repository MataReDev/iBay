using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public float Price { get; set; }
        public bool Available { get; set; }
        public DateTime Added_time { get; set; }

    }
}