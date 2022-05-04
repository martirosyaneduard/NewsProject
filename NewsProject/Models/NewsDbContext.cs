using Microsoft.EntityFrameworkCore;

namespace NewsProject.Models
{
    public class NewsDbContext:DbContext
    {
        public DbSet<Category>? Categories { get; set; }
        public DbSet<New>? News { get; set; }
        public NewsDbContext(DbContextOptions<NewsDbContext> options):base(options)
        {

        }
    }
}
