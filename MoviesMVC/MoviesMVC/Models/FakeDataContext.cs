using Microsoft.EntityFrameworkCore;
using MoviesApi.Models;

namespace MoviesMvc.Models
{
    public class FakeContext : DbContext
    {
        public FakeContext(DbContextOptions<FakeContext> options)
            : base(options)
        {
        }

        public DbSet<MoviesItem> MoviesItems { get; set; }

        public DbSet<MoviesApi.Models.MoviesItem> MoviesItem { get; set; }
    }
}

