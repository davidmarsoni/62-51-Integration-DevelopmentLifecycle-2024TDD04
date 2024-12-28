using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class RoomAccessContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Access> Accesses { get; set; }
        public DbSet<RoomAccessLog> RoomAccessLogs { get; set; }

        public RoomAccessContext(DbContextOptions<RoomAccessContext> options) : base(options)
        {
        }

        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
            if (environment == "Development")
            {
                builder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=RoomAccess");
            }
        }
        
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many for User-UserGroups-Group
            modelBuilder.Entity<User>()
                .HasMany(e => e.Groups)
                .WithMany(e => e.Users)
                .UsingEntity<UserGroup>();
        }
    }
}
