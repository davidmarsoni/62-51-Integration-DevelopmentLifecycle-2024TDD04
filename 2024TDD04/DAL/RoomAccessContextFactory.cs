using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class RoomAccessContextFactory : IDesignTimeDbContextFactory<RoomAccessContext>
    {
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public RoomAccessContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RoomAccessContext>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RoomAccess;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

            return new RoomAccessContext(optionsBuilder.Options);
        }
    }
}
