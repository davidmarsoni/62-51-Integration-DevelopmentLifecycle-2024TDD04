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
        public async void AccessAsync_UserInGroupWithAccess_ShouldReturnRoomAccessDTO()
        {
            // Arrange
            RoomAccessDTO accessDTO = new RoomAccessDTO
            {
                UserId = 1, // User in group with access
                RoomId = 1  // Room that the group has access to
            };

            // Act
            var result = await _roomAccessesController.AccessAsync(accessDTO);

            // Assert
            Assert.IsType<ActionResult<RoomAccessDTO>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(accessDTO.UserId, result.Value.UserId);
            Assert.Equal(accessDTO.RoomId, result.Value.RoomId);
        }

        [Fact]
        public async void AccessAsync_UserInGroupWithoutAccess_ShouldReturnNull()
        {
            // Arrange
            RoomAccessDTO accessDTO = new RoomAccessDTO
            {
                UserId = 1,
                RoomId = 2  // Room that the group doesn't have access to
            };

            // Act
            var result = await _roomAccessesController.AccessAsync(accessDTO);

            // Assert
            Assert.IsType<ActionResult<RoomAccessDTO>>(result);
            Assert.Null(result.Result);
        }

        [Fact]
        public async void AccessAsync_UserNotInAnyGroup_ShouldReturnNullAction()
        {
            // Arrange
            RoomAccessDTO accessDTO = new RoomAccessDTO
            {
                UserId = 4, // User not in any group
                RoomId = 1
            };

            // Act
            var result = await _roomAccessesController.AccessAsync(accessDTO);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async void AccessAsync_UserDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            RoomAccessDTO accessDTO = new RoomAccessDTO
            {
                UserId = 999, // Non-existent user
                RoomId = 1
            };

            // Act
            var result = await _roomAccessesController.AccessAsync(accessDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void AccessAsync_UserIsDeleted_ShouldReturnForbid()
        {
            // Arrange
            RoomAccessDTO accessDTO = new RoomAccessDTO
            {
                UserId = 3, // Deleted user
                RoomId = 1
            };

            // Act
            var result = await _roomAccessesController.AccessAsync(accessDTO);

            // Assert
            Assert.IsType<ForbidResult>(result.Result);
        }

        [Fact]
        public async void AccessAsync_RoomDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            RoomAccessDTO accessDTO = new RoomAccessDTO
            {
                UserId = 1,
                RoomId = 999 // Non-existent room
            };

            // Act
            var result = await _roomAccessesController.AccessAsync(accessDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        #endregion
    }
}
