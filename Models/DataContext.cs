using Microsoft.EntityFrameworkCore;

namespace JustManAdmin.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options):base(options)
        {
        }
        public DbSet<MainCategory> MainCategories { get; set; }
        public DbSet<Article> Articles {get;set;}
    }
}