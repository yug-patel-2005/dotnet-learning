using CRUDproject.Models.AuthUser;
using CRUDproject.Models.RoleDto;
using CRUDproject.Models.UserRoles;
using Microsoft.EntityFrameworkCore;

namespace CRUDproject.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

     
        public DbSet<JobEntity> Jobs { get; set; }   
        public DbSet<User> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<RoleEntity>().HasData(
                new RoleEntity { RoleId = 1, RoleName = "Admin" },
                new RoleEntity { RoleId = 2, RoleName = "User" }
            );
        }
    }
}