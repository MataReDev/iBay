using Microsoft.EntityFrameworkCore;
using System;
using ClassLibrary;

namespace iBay
{
    public class Context : DbContext
    {
        public DbSet<User> User { get; set; }
        public Context(DbContextOptions<Context> options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(@"Data Source=PC-MATHYS-PORT\SQLEXPRESS;Initial Catalog=ibay;Integrated Security=True;Trust Server Certificate=true");
    }
}