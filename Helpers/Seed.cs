using System.Linq;
using JustManAdmin.Models;

namespace JustManAdmin.Helpers
{
    public class Seed
    {
        public static void SeedData(DataContext context)
        {
            if(!context.Users.Any())
            {
                var user = new User()
                {
                    Login = "Admin",
                    Password = Hashing.CreateMD5("1234")
                };
                user.Role = context.Roles.First(p=> p.Id == 1);
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}