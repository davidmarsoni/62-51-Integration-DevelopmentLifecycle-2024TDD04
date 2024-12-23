using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace _2024TDD04.DAL.Tests.WebAPI
{
    public class InMemoryRoomContext
    {
        public static RoomAccessContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<RoomAccessContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new RoomAccessContext(options);

            // Create the database
            context.Database.EnsureCreated();

            // Seed data
            context.Users.AddRange(
                new User { Id = 1, Username = "Widmer", IsDeleted = false },
                new User { Id = 2, Username = "David", IsDeleted = false },
                new User { Id = 3, Username = "Zanya", IsDeleted = true },
                new User { Id = 4, Username = "Mathias", IsDeleted = true }
            );
            context.Groups.AddRange(
                new Group { Id = 1, Name = "Teachers", Acronym = "TCH" },
                new Group { Id = 2, Name = "Students", Acronym = "STU" },
                new Group { Id = 3, Name = "NoRoomGroup", Acronym = "NRG" },
                new Group { Id = 4, Name = "NoUserGroup", Acronym = "NUG" }
            );
            context.UserGroups.AddRange(
                new UserGroup { Id = 1, UserId = 1, GroupId = 1 },
                new UserGroup { Id = 2, UserId = 2, GroupId = 2 },
                new UserGroup { Id = 3, UserId = 3, GroupId = 3 }
            );
            context.Rooms.AddRange(
                new Room { Id = 1, Name = "Room 301", RoomAbreviation = "301", IsDeleted = false },
                new Room { Id = 2, Name = "Student Study Room", RoomAbreviation = "SSR", IsDeleted = false },
                new Room { Id = 3, Name = "Utility Closet R303", RoomAbreviation = "R303", IsDeleted = true }
            );
            context.Accesses.AddRange(
                new Access { Id = 1, GroupId = 1, RoomId = 1 },
                new Access { Id = 2, GroupId = 1, RoomId = 2 },
                new Access { Id = 3, GroupId = 2, RoomId = 2 }
            );
            context.RoomAccessLogs.AddRange(
                new RoomAccessLog { Id = 1, UserId = 1, RoomId = 1, Room = "Utility Closet R303", Info = "Access denied" },
                new RoomAccessLog { Id = 2, UserId = 2, RoomId = 1, Room = "Utility Closet R303", Info = "Access granted" }
            );

            context.SaveChanges();
            return context;
        }
    }
}