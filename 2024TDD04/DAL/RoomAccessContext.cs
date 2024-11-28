﻿using DAL.Models;
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

        public RoomAccessContext(DbContextOptions<RoomAccessContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=RoomRoom");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure many-to-many for User-UserGroups-Group
            modelBuilder.Entity<User>()
                .HasMany(e => e.Groups)
                .WithMany(e => e.Users)
                .UsingEntity<UserGroup>();

            // Configure composed primary key for Access and UserGroup
            modelBuilder.Entity<Access>()
                .HasKey(e => new { e.RoomId, e.GroupId });
            modelBuilder.Entity<UserGroup>()
                .HasKey(e => new { e.UserId, e.GroupId });
        }
    }
}