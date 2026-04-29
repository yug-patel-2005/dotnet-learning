using Microsoft.EntityFrameworkCore;
using CRUDproject.Models.AuthUser;

namespace CRUDproject.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<JobEntity> Jobs { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
