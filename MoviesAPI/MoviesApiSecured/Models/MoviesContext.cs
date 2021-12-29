using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Models
{
    public class MoviesContext : DbContext
    {
        public MoviesContext(DbContextOptions<MoviesContext> options)
            : base(options)
        {
        }

        public DbSet<MoviesItem> MoviesItems { get; set; }
    }
}