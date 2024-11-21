using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class RoomRoomContextFactory : IDesignTimeDbContextFactory<RoomRoom_Context>
    {
        public RoomRoom_Context CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RoomRoom_Context>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RoomRoom;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

            return new RoomRoom_Context(optionsBuilder.Options);
        }
    }
}
