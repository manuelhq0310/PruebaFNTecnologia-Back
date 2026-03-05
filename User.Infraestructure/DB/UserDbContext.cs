using Microsoft.EntityFrameworkCore;
using UserService.Domain.Models;

namespace UserService.Infrastructure.DB
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
