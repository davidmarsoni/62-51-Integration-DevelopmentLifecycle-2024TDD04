using DAL;
using Microsoft.EntityFrameworkCore;

namespace _2024TDD04.DAL.Tests.Models
{
    public class RoomAccessContextTests
    {
        [Fact]
        public void Context_WhenGivenInMemoryCreation_ShouldBuildContext()
        {
            var options = new DbContextOptionsBuilder<RoomAccessContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new RoomAccessContext(options))
            {
                Assert.NotNull(context);
            }
        }
    }
}