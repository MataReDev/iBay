using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary
{
    [Table("PaymentHistory")]
    public class PaymentHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public float Amount { get; set; }
    }
}
