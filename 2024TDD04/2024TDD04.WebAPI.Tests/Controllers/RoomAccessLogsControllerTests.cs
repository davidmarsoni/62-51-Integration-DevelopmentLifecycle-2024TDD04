using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Controllers;
using _2024TDD04.WebAPI.Tests.GeneralSetup;

namespace _2024TDD04.WebAPI.Tests.Controllers
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
        public async Task GetRoomAccessLogs_WhenGivenAllParametersNull_ShouldReturnRooms()
        {
            // Arrange

            // Act
            var result = await roomAccessLogsController.GetRoomAccessLogs(null, null, null);

            // Assert
            var roomAccessLogs = Assert.IsType<List<RoomAccessLogDTO>>(result.Value);
            Assert.Equal(2, roomAccessLogs.Count);
        }

        [Fact]
        public async Task GetRoomAccessLogs_WhenNoLogsInDB_ShouldReturnEmptyList()
        {
            // Arrange
            _testDbContext.RoomAccessLogs.RemoveRange(_testDbContext.RoomAccessLogs);
            _testDbContext.SaveChanges();

            // Act
            var result = await roomAccessLogsController.GetRoomAccessLogs();

            // Assert
            var roomAccessLogs = Assert.IsType<List<RoomAccessLogDTO>>(result.Value);
            Assert.Empty(roomAccessLogs);
        }

        [Fact]
        public async Task GetRoomAccessLogs_WhenGivenSingleLog_ShouldReturnSingleRoomAccessLog(){
            // Arrange
            var logNumber = 1;

            // Act
            var result = await roomAccessLogsController.GetRoomAccessLogs(logNumber);

            // Assert
            var roomAccessLogs = Assert.IsType<List<RoomAccessLogDTO>>(result.Value);
            Assert.Single(roomAccessLogs);
        }

        [Fact]
        public async Task GetRoomAccessLogs_WhenGivenThreeLog_ShouldReturnThreeOrAllRoomAccessLogs(){
            // Arrange
            var logNumber = 3;

            // Act
            var result = await roomAccessLogsController.GetRoomAccessLogs(logNumber);

            // Assert
            var roomAccessLogs = Assert.IsType<List<RoomAccessLogDTO>>(result.Value);
            Assert.Equal(2, roomAccessLogs.Count);
        }

        [Fact]
        public async Task GetRoomAccessLogs_WhenGivenOffset_ShouldReturnRoomAccessLogOffsetByOne(){
            // Arrange
            var offset = 1;

            // Act
            var result = await roomAccessLogsController.GetRoomAccessLogs(null, offset);

            // Assert
            var roomAccessLogs = Assert.IsType<List<RoomAccessLogDTO>>(result.Value);
            Assert.Single(roomAccessLogs);
            Assert.Equal(1, roomAccessLogs[0].Id);
        }

        [Fact]
        public async Task GetRoomAccessLogs_WhenGivenAscendingOrder_ShouldReturnRoomAccessLogsInAscendingOrder(){
            // Arrange

            // Act
            var result = await roomAccessLogsController.GetRoomAccessLogs(null, null, "asc");

            // Assert
            var roomAccessLogs = Assert.IsType<List<RoomAccessLogDTO>>(result.Value);
            Assert.Equal(2, roomAccessLogs.Count);
            Assert.Equal(1, roomAccessLogs[0].Id);
            Assert.Equal(2, roomAccessLogs[1].Id);
        }

        [Fact]
        public async Task GetRoomAccessLogs_WhenGivenInvalidParameter_ShouldReturnDefaultAmountOfLogsInDescendingOrder(){
            // Arrange

            // Act
            var result = await roomAccessLogsController.GetRoomAccessLogs(-1, -1, "invalid");

            // Assert
            var roomAccessLogs = Assert.IsType<List<RoomAccessLogDTO>>(result.Value);
            Assert.Equal(2, roomAccessLogs.Count);
            Assert.Equal(2, roomAccessLogs[0].Id);
            Assert.Equal(1, roomAccessLogs[1].Id);
        }

        #endregion


        
    }
}
