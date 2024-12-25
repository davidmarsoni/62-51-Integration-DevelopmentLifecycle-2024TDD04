using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers;

namespace _2024TDD04.DAL.Tests.WebAPI
{
    public class RoomAccessLogsControllerTests
    {
        private readonly RoomAccessLogsController roomAccessLogsController;
        private readonly RoomAccessContext _testDbContext;

        public RoomAccessLogsControllerTests()
        {
            _testDbContext = InMemoryRoomContext.CreateInMemoryContext();
            roomAccessLogsController = new RoomAccessLogsController(_testDbContext);
        }

        #region GetRoomAccessLogs
        [Fact]
        public async void GetRoomAccessLogs_WhenAllParametersNull_ReturnsAllRoomAccessLogsUsingDefaultValues()
        {
            // Act
            var result = await roomAccessLogsController.GetRoomAccessLogsAsync(null, null, null);

            // Assert
            var roomAccessLogs = Assert.IsType<List<RoomAccessLogDTO>>(result.Value);
            Assert.Equal(2, roomAccessLogs.Count);
        }

        [Fact]
        public async void GetRoomAccessLogs_NoLogsInTheDatabase_ReturnsEmptyList()
        {
            // Arrange
            _testDbContext.RoomAccessLogs.RemoveRange(_testDbContext.RoomAccessLogs);
            _testDbContext.SaveChanges();

            // Act
            var result = await roomAccessLogsController.GetRoomAccessLogsAsync(null, null, null);

            // Assert
            var roomAccessLogs = Assert.IsType<List<RoomAccessLogDTO>>(result.Value);
            Assert.Empty(roomAccessLogs);
        }

        [Fact]
        public async void GetRoomAccessLogs_WhenSingleLogsAsked_ReturnsRoomAccessLogsSingle(){
            // Arrange

            // Act
            var result = await roomAccessLogsController.GetRoomAccessLogsAsync(1, null, null);

            // Assert
            var roomAccessLogs = Assert.IsType<List<RoomAccessLogDTO>>(result.Value);
            Assert.Single(roomAccessLogs);
        }

        [Fact]
        public async void GetRoomAccessLogs_WhenThreeLogsAsked_ReturnsAllRoomAccessLogs(){
            // Arrange

            // Act
            var result = await roomAccessLogsController.GetRoomAccessLogsAsync(3, null, null);

            // Assert
            var roomAccessLogs = Assert.IsType<List<RoomAccessLogDTO>>(result.Value);
            Assert.Equal(2, roomAccessLogs.Count);
        }

        [Fact]
        public async void GetRoomAccessLogs_WhenOffsetIsGiven_ReturnsRoomAccessLogsSingleOffset(){
            // Arrange

            // Act
            var result = await roomAccessLogsController.GetRoomAccessLogsAsync(null, 1, null);

            // Assert
            var roomAccessLogs = Assert.IsType<List<RoomAccessLogDTO>>(result.Value);
            Assert.Single(roomAccessLogs);
        }

        [Fact]
        public async void GetRoomAccessLogs_WhenInvalidParameter_ReturnsDefaultAmmountOfLogs(){
            // Arrange

            // Act
            var result = await roomAccessLogsController.GetRoomAccessLogsAsync(-1, -1, "invalid");

            // Assert
            var roomAccessLogs = Assert.IsType<List<RoomAccessLogDTO>>(result.Value);
            Assert.Equal(2, roomAccessLogs.Count);
        }

        #endregion


        
    }
}
