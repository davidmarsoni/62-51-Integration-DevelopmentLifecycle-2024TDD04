using DAL;
using DAL.Models;
using DTO;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;
using Xunit;
using _2024TDD04.WebAPI.Tests.GeneralSetup;

namespace _2024TDD04.WebAPI.Tests.Controllers
{
    public class RoomAccessesControllerTests
    {
        private readonly RoomAccessesController _roomAccessesController;
        private readonly RoomAccessContext _testDbContext;

        public RoomAccessesControllerTests()
        {
            _testDbContext = InMemoryRoomContext.CreateInMemoryContext();
            _roomAccessesController = new RoomAccessesController(_testDbContext);
        }

        #region AccessAsync

        [Fact]
        public async Task AccessAsync_WhenGivenUserWithAccess_ShouldReturnRoomAccessDTO()
        {
            // Arrange
            var roomId = 1; // Room that the group has access to
            var userId = 1; // User in group with access

            // Act
            var result = await _roomAccessesController.AccessRoom(roomId, userId);

            // Assert
            Assert.IsType<ActionResult<RoomAccessDTO>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(roomId, result.Value.RoomId);
            Assert.Equal(userId, result.Value.UserId);
        }

        [Fact]
        public async Task AccessAsync_WhenGivenUserWithoutAccess_ShouldReturnNull()
        {
            // Arrange
            var roomId = 2; // Room that the group doesn't have access to
            var userId = 1;

            // Act
            var result = await _roomAccessesController.AccessRoom(roomId, userId);

            // Assert
            Assert.IsType<ActionResult<RoomAccessDTO>>(result);
            Assert.Null(result.Result);
        }

        [Fact]
        public async Task AccessAsync_WhenGivenUserWithoutGroup_ShouldReturnNullAction()
        {
            // Arrange
            var roomId = 1;
            var userId = 4; // User not in any group

            // Act
            var result = await _roomAccessesController.AccessRoom(roomId, userId);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async Task AccessAsync_WhenGivenDeletedUser_ShouldReturnForbid()
        {
            // Arrange
            var roomId = 1;
            var userId = 3; // Deleted user

            // Act
            var result = await _roomAccessesController.AccessRoom(roomId, userId);

            // Assert
            Assert.IsType<ForbidResult>(result.Result);
        }

        [Fact]
        public async Task AccessAsync_WhenGivenNonExistentUser_ShouldReturnNotFound()
        {
            // Arrange
            var roomId = 1;
            var userId = 999; // Non-existent user

            // Act
            var result = await _roomAccessesController.AccessRoom(roomId, userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task AccessAsync_WhenGivenNonExistentRoom_ShouldReturnNotFound()
        {
            // Arrange
            var roomId = 999; // Non-existent room
            var userId = 1;

            // Act
            var result = await _roomAccessesController.AccessRoom(roomId, userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        #endregion
    }
}
