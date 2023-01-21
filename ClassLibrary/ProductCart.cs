using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class ProductCart
    {
        public int Id { get; set; }
        public virtual Cart cart { get; set; }
        public virtual Product product { get; set; }
    }
}
