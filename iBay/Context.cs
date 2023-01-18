using Microsoft.EntityFrameworkCore;
using System;
using ClassLibrary;

namespace iBay
{
    public class Context : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Cart> Cart { get; set; }
        public Context(DbContextOptions<Context> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(@"Data Source=DESKTOP-0M2IDL4\Yoann;Initial Catalog=ibay;Integrated Security=True;Trust Server Certificate=true");
    }
}