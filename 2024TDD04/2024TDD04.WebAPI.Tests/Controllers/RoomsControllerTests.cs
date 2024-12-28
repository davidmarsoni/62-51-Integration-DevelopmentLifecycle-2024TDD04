using DAL;
using DTO;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace _2024TDD04.DAL.Tests.WebAPI
{
    public class RoomsControllerTests
    {
        private readonly RoomsController _roomsController;
        private readonly RoomAccessContext _testDbContext;

        public RoomsControllerTests()
        {
            _testDbContext = InMemoryRoomContext.CreateInMemoryContext();
            _roomsController = new RoomsController(_testDbContext);
        }

        #region GetRooms

        [Fact]
        public async void GetRooms_WhenHasRooms_ShouldReturnListOfRooms()
        {
            // Arrange

            // Act
            var result = await _roomsController.GetRooms();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<RoomDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(3, result.Value.Count());
        }

        public async void GetRooms_WhenHasNoRooms_ShouldReturnEmptyList()
        {
            // Arrange
            _testDbContext.Rooms.RemoveRange(_testDbContext.Rooms);
            _testDbContext.SaveChanges();

            // Act
            var result = await _roomsController.GetRooms();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<RoomDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
        }

        #endregion

        #region GetRoomsActive

        [Fact]
        public async void GetRoomsActive_WhenHasRooms_ShouldReturnListOfRooms()
        {
            // Arrange

            // Act
            var result = await _roomsController.GetRoomsActive();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<RoomDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Count());
        }

        [Fact]
        public async void GetRoomsActive_WhenHasNoRooms_ShouldReturnEmptyList()
        {
            // Arrange
            _testDbContext.Rooms.RemoveRange(_testDbContext.Rooms);
            _testDbContext.SaveChanges();

            // Act
            var result = await _roomsController.GetRoomsActive();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<RoomDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
        }

        #endregion

        #region GetRoom

        [Fact]
        public async void GetRoom_RoomExists_ShouldReturnRoomDTO()
        {
            // Arrange

            // Act
            var result = await _roomsController.GetRoom(1);

            // Assert
            Assert.IsType<ActionResult<RoomDTO>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
        }

        [Fact]
        public async void GetRoom_RoomDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange

            // Act
            var result = await _roomsController.GetRoom(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        #endregion

        #region PutRoom

        [Fact]
        public async void PutRoom_RoomExists_ShouldReturnNoContentAndEntryIsUpdated()
        {
            // Arrange
            RoomDTO roomDTO = new RoomDTO
            {
                Id = 1,
                Name = "Updated Room",
                RoomAbreviation = "UR1"
            };

            // Act
            var result = await _roomsController.PutRoom(1, roomDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);

            var room = await _testDbContext.Rooms.FindAsync(1);
            Assert.NotNull(room);
            Assert.Equal(roomDTO.Name, room.Name);
            Assert.Equal(roomDTO.RoomAbreviation, room.RoomAbreviation);
        }

        [Fact]
        public async void PutRoom_RoomDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            RoomDTO roomDTO = new RoomDTO
            {
                Id = 999,
                Name = "Test Room",
                RoomAbreviation = "TR1"
            };

            // Act
            var result = await _roomsController.PutRoom(999, roomDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void PutRoom_IdMismatch_ShouldReturnBadRequest()
        {
            // Arrange
            RoomDTO roomDTO = new RoomDTO
            {
                Id = 1,
                Name = "Test Room",
                RoomAbreviation = "TR1"
            };

            // Act
            var result = await _roomsController.PutRoom(2, roomDTO);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        #endregion

        #region PostRoom

        [Fact]
        public async void PostRoom_ValidRoom_ShouldReturnCreatedAtAction()
        {
            // Arrange
            RoomDTO roomDTO = new RoomDTO
            {
                Name = "New Room",
                RoomAbreviation = "NR1"
            };

            // Act
            var result = await _roomsController.PostRoom(roomDTO);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async void PostRoom_DuplicateName_ShouldReturnConflict()
        {
            // Arrange
            RoomDTO roomDTO = new RoomDTO
            {
                Name = "Room 301",  // Assuming this exists in test data
                RoomAbreviation = "NR1"
            };

            // Act
            var result = await _roomsController.PostRoom(roomDTO);

            // Assert
            Assert.IsType<ConflictResult>(result.Result);
        }

        [Fact]
        public async void PostRoom_DuplicateAbreviation_ShouldReturnConflict()
        {
            // Arrange
            RoomDTO roomDTO = new RoomDTO
            {
                Name = "New Room",
                RoomAbreviation = "301"  // Assuming this exists in test data
            };

            // Act
            var result = await _roomsController.PostRoom(roomDTO);

            // Assert
            Assert.IsType<ConflictResult>(result.Result);
        }

        [Fact]
        public async void PostRoom_NullDTO_ShouldReturnBadRequest()
        {
            // Act
            var result = await _roomsController.PostRoom(null);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        #endregion

        #region DeleteRoom

        [Fact]
        public async void DeleteRoom_RoomExists_ShouldSetIsDeletedAndReturnNoContent()
        {
            // Arrange

            // Act
            var result = await _roomsController.DeleteRoom(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var room = await _testDbContext.Rooms.FindAsync(1);
            Assert.True(room.IsDeleted);
        }

        [Fact]
        public async void DeleteRoom_RoomDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange

            // Act
            var result = await _roomsController.DeleteRoom(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion

        #region RoomNameExists

        [Fact]
        public async void RoomNameExists_NameExists_ShouldReturnTrue()
        {
            // Arrange

            // Act
            var result = await _roomsController.RoomNameExists("Room 301");

            // Assert
            Assert.IsType<ActionResult<bool>>(result);
            Assert.True(result.Value);
        }

        [Fact]
        public async void RoomNameExists_NameDoesNotExist_ShouldReturnFalse()
        {
            // Arrange

            // Act
            var result = await _roomsController.RoomNameExists("NonExistentRoom");

            // Assert
            Assert.IsType<ActionResult<bool>>(result);
            Assert.False(result.Value);
        }

        #endregion

        #region RoomAbreviationExists

        [Fact]
        public async void RoomAbreviationExists_AbreviationExists_ShouldReturnTrue()
        {
            // Arrange

            // Act
            var result = await _roomsController.RoomAbreviationExists("301");

            // Assert
            Assert.IsType<ActionResult<bool>>(result);
            Assert.True(result.Value);
        }

        [Fact]
        public async void RoomAbreviationExists_AbreviationDoesNotExist_ShouldReturnFalse()
        {
            // Arrange

            // Act
            var result = await _roomsController.RoomAbreviationExists("XXX");

            // Assert
            Assert.IsType<ActionResult<bool>>(result);
            Assert.False(result.Value);
        }

        #endregion
    }
}