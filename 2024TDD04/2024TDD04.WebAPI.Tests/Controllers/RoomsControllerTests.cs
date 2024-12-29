using DAL;
using DTO;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;
using _2024TDD04.WebAPI.Tests.GeneralSetup;

namespace _2024TDD04.WebAPI.Tests.Controllers
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
        public async void GetRooms_WhenRoomsInDB_ShouldReturnListOfRooms()
        {
            // Arrange

            // Act
            var result = await _roomsController.GetRooms();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<RoomDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(3, result.Value.Count());
        }

        public async void GetRooms_WhenNoRoomsInDB_ShouldReturnEmptyList()
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
        public async void GetRoomsActive_WhenActiveRoomsInDB_ShouldReturnListOfActiveRooms()
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
        public async void GetRoomsActive_WhenNoActiveRoomsInDB_ShouldReturnEmptyList()
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
        public async void GetRoom_WhenGivenValidRoom_ShouldReturnRoomDTO()
        {
            // Arrange
            var roomId = 1;

            // Act
            var result = await _roomsController.GetRoom(roomId);

            // Assert
            Assert.IsType<ActionResult<RoomDTO>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
        }

        [Fact]
        public async void GetRoom_WhenGivenNonExistent_ShouldReturnNotFound()
        {
            // Arrange
            var roomId = 999;

            // Act
            var result = await _roomsController.GetRoom(roomId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        #endregion

        #region PutRoom

        [Fact]
        public async void PutRoom_WhenGivenValidRoom_ShouldReturnNoContentAndUpdateRoom()
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
        public async void PutRoom_WhenGivenNonExistentRoom_ShouldReturnNotFound()
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
        public async void PutRoom_WhenGivenNonMatchingData_ShouldReturnBadRequest()
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

        [Fact]
        public async void PutRoom_WhenGivenRoomWithDuplicateName_ShouldReturnConflict()
        {
            // Arrange
            RoomDTO roomDTO = new RoomDTO
            {
                Id = 1,
                Name = "Room 301",  // Assuming this exists in test data
                RoomAbreviation = "TR1"
            };

            // Act
            var result = await _roomsController.PutRoom(1, roomDTO);

            // Assert
            Assert.IsType<ConflictResult>(result);
        }

        [Fact]
        public async void PutRoom_WhenGivenRoomWithDuplicateAbreviation_ShouldReturnConflict()
        {
            // Arrange
            RoomDTO roomDTO = new RoomDTO
            {
                Id = 1,
                Name = "Test Room",
                RoomAbreviation = "301"  // Assuming this exists in test data
            };

            // Act
            var result = await _roomsController.PutRoom(1, roomDTO);

            // Assert
            Assert.IsType<ConflictResult>(result);
        }

        [Fact]
        public async void PutRoom_WhenGivenRoomWithBlankAbreviation_ShouldReturnNoContentAndUpdateRoom()
        {
            // Arrange
            RoomDTO roomDTO = new RoomDTO
            {
                Id = 1,
                Name = "Test Room",
                RoomAbreviation = ""
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

        #endregion

        #region PostRoom

        [Fact]
        public async void PostRoom_WhenGivenValidRoom_ShouldReturnCreatedAtActionAndCreateRoom()
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
        public async void PostRoom_WhenGivenRoomWithDuplicateName_ShouldReturnConflict()
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
        public async void PostRoom_WhenGivenRoomWithDuplicateAbreviation_ShouldReturnConflict()
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
        public async void PostRoom_WhenGivenRoomWithBlankAbreviation_ShouldReturnCreatedAtActionAndCreateRoom()
        {
            // Arrange
            RoomDTO roomDTO = new RoomDTO
            {
                Name = "New Room",
                RoomAbreviation = ""
            };

            // Act
            var result = await _roomsController.PostRoom(roomDTO);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        #endregion

        #region DeleteRoom

        [Fact]
        public async void DeleteRoom_WhenGivenValidRoom_ShouldReturnNoContentAndUpdateRoom()
        {
            // Arrange
            var roomId = 1;

            // Act
            var result = await _roomsController.DeleteRoom(roomId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            var room = await _testDbContext.Rooms.FindAsync(roomId);
            Assert.True(room.IsDeleted);
        }

        [Fact]
        public async void DeleteRoom_WhenGivenNonExistentRoom_ShouldReturnNotFound()
        {
            // Arrange
            var roomId = 999;

            // Act
            var result = await _roomsController.DeleteRoom(roomId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion

        #region RoomNameExists

        [Fact]
        public async void RoomNameExists_WhenGivenExistingName_ShouldReturnTrue()
        {
            // Arrange
            var existingRoomName = "Room 301";

            // Act
            var result = await _roomsController.RoomNameExists(existingRoomName);

            // Assert
            Assert.IsType<ActionResult<bool>>(result);
            Assert.True(result.Value);
        }

        [Fact]
        public async void RoomNameExists_WhenGivenNonExistentName_ShouldReturnFalse()
        {
            // Arrange
            var nonExistentRoomName = "NonExistentRoom";

            // Act
            var result = await _roomsController.RoomNameExists(nonExistentRoomName);

            // Assert
            Assert.IsType<ActionResult<bool>>(result);
            Assert.False(result.Value);
        }

        #endregion

        #region RoomAbreviationExists

        [Fact]
        public async void RoomAbreviationExists_WhenGivenExistingAbreviation_ShouldReturnTrue()
        {
            // Arrange
            var existingRoomAbreviation = "301";

            // Act
            var result = await _roomsController.RoomAbreviationExists(existingRoomAbreviation);

            // Assert
            Assert.IsType<ActionResult<bool>>(result);
            Assert.True(result.Value);
        }

        [Fact]
        public async void RoomAbreviationExists_WhenGivenNonExistentAbreviation_ShouldReturnFalse()
        {
            // Arrange
            var nonExistentRoomAbreviation = "XXX";

            // Act
            var result = await _roomsController.RoomAbreviationExists(nonExistentRoomAbreviation);

            // Assert
            Assert.IsType<ActionResult<bool>>(result);
            Assert.False(result.Value);
        }

        #endregion
    }
}