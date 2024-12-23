using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public interface IRoomAccessContext
    {
        DbSet<Room> Rooms { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Group> Groups { get; set; }
        DbSet<UserGroup> UserGroups { get; set; }
        DbSet<Access> Accesses { get; set; }
        DbSet<RoomAccessLog> RoomAccessLogs { get; set; }
    }
}
