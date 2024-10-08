using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class RoomRoom_Context : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Access> Accesses { get; set; }

        public RoomRoom_Context(DbContextOptions<RoomRoom_Context> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=RoomRoom");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many
            modelBuilder.Entity<User>()
                .HasMany(e => e.Groups)
                .WithMany(e => e.Users)
                .UsingEntity<UserGroup>();
        }
    }
}
