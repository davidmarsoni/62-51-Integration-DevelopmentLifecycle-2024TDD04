using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace TTD04_2024.DAL.Tests.WebAPI
{
    public class InMemoryRoomContext
    {
        private static RoomAccessContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<RoomAccessContext>()
                .UseInMemoryDatabase("RoomAccessTestDB")
                .Options;
            var context = new RoomAccessContext(options);

            // Seed data
            context.Users.AddRange(
                new User { Id = 1, Username = "Mathias", IsDeleted = false },
                new User { Id = 2, Username = "David", IsDeleted = false },
                new User { Id = 3, Username = "Zanya", IsDeleted = true }
            );
            context.Groups.AddRange(
                new Group { Id = 1, Name = "Teachers", Acronym = "TCH" },
                new Group { Id = 2, Name = "Students", Acronym = "STU" }
            );
            context.UserGroups.AddRange(
                new UserGroup { Id = 1, UserId = 1, GroupId = 1 },
                new UserGroup { Id = 2, UserId = 2, GroupId = 2 }
            );
            context.Rooms.AddRange(
                new Room { Id = 1, Name = "Room 301", RoomAbreviation = "301", IsDeleted = false },
                new Room { Id = 2, Name = "Room 302", RoomAbreviation = "302", IsDeleted = false },
                new Room { Id = 3, Name = "Utility Closet R303", RoomAbreviation = "R303", IsDeleted = true }
            );
            context.Accesses.AddRange(
                new Access { Id = 1, GroupId = 1, RoomId = 1 },
                new Access { Id = 2, GroupId = 2, RoomId = 2 }
            );
            context.RoomAccessLogs.AddRange(
                new RoomAccessLog { Id = 1, UserId = 1, RoomId = 3, Room = "Utility Closet R303", Info = "Access denied" },
                new RoomAccessLog { Id = 2, UserId = 2, RoomId = 3, Room = "Utility Closet R303", Info = "Access granted" }
            );

            context.SaveChanges();
            return context;
        }
    }
}