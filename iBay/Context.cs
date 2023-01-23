using Microsoft.EntityFrameworkCore;
using ClassLibrary;

namespace iBay
{
    /// <summary>
    /// Context
    /// </summary>
    public class Context : DbContext
    {
        /// <summary>
        /// Context User
        /// </summary>
        public DbSet<User> User { get; set; }
        /// <summary>
        /// Context Product
        /// </summary>
        public DbSet<Product> Product { get; set; }
        /// <summary>
        /// Context Cart
        /// </summary>
        public DbSet<Cart> Cart { get; set; }
        /// <summary>
        /// Context ProductCart
        /// </summary>
        public DbSet<ProductCart> ProductCart { get; set; }
        /// <summary>
        /// Context PaymentHistory
        /// </summary>
        public DbSet<PaymentHistory> PaymentHistory { get; set; }

        /// <summary>
        /// Constructeur Context
        /// </summary>
        /// <param name="options"></param>
        public Context(DbContextOptions<Context> options)
            : base(options) { }

        /// <summary>
        /// Configuration BDD
        /// </summary>
        /// <param name="options"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(@"Data Source=DESKTOP-0M2IDL4\sqlexpress;Initial Catalog=ibay;Integrated Security=True;Trust Server Certificate=true");
    }
}