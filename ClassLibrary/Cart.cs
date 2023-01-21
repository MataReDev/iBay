using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.X509Certificates;

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