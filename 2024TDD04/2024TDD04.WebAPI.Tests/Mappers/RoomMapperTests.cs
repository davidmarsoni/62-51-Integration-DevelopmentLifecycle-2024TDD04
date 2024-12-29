
using DAL.Models;
using DTO;
using WebAPI.Mapper;
using Xunit;

namespace _2024TDD04.WebAPI.Tests.Mappers
{
    public class RoomMapperTests
    {
        #region ToDTO

        [Fact]
        public static void ToDTO_WhenGivenRoom_ShouldReturnRoomDTO()
        {
            // Arrange
            Room room = new Room
            {
                Id = 1,
                Name = "TestRoom",
                RoomAbreviation = "TR",
                IsDeleted = false
            };

            // Act
            var result = RoomMapper.toDTO(room);

            // Assert
            Assert.IsType<RoomDTO>(result);
            Assert.Equal(room.Id, result.Id);
            Assert.Equal(room.Name, result.Name);
            Assert.Equal(room.RoomAbreviation, result.RoomAbreviation);
            Assert.Equal(room.IsDeleted, result.IsDeleted);
        }

        #endregion

        #region ToDAL

        [Fact]
        public static void ToDAL_WhenGivenRoomDTO_ShouldReturnRoom()
        {
            // Arrange
            RoomDTO roomDTO = new RoomDTO
            {
                Id = 1,
                Name = "TestRoomDTO",
                RoomAbreviation = "TRD",
                IsDeleted = false
            };

            // Act
            var result = RoomMapper.toDAL(roomDTO);

            // Assert
            Assert.IsType<Room>(result);
            Assert.Equal(roomDTO.Id, result.Id);
            Assert.Equal(roomDTO.Name, result.Name);
            Assert.Equal(roomDTO.RoomAbreviation, result.RoomAbreviation);
            Assert.Equal(roomDTO.IsDeleted, result.IsDeleted);
        }

        #endregion
    }
}