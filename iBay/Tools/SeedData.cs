using ClassLibrary;
using Microsoft.EntityFrameworkCore;

namespace iBay.Tools
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new Context(
                serviceProvider.GetRequiredService<DbContextOptions<Context>>()))

            {
                //Look for any classroom
                if (context.Cart.Any())
                {
                    return;
                }

                //New Teacher
                User user = new User
                {
                   
                    Email = "blabla@gmail.com",
                    Pseudo = "test",
                    Password = "testt",
                    Role= "seller",
                };
                context.SaveChanges();
                //New Classroom
                context.Cart.AddRange(
                    new Cart
                    {
                        UserId = user.Id,
                        DateValidation = DateTime.Now,
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
