
using DAL.Models;
using DTO;
using WebAPI.Mapper;
using Xunit;

namespace _2024TDD04.WebAPI.Tests.Mappers
{
    public class RoomAccessLogMapperTests
    {
        #region ToDTO

        [Fact]
        public void ToDTO_WhenGivenRoomAccessLog_ShouldReturnRoomAccessLogDTO()
        {
            // Arrange
            RoomAccessLog log = new RoomAccessLog
            {
                Id = 1,
                RoomId = 2,
                Room = new Room { Id = 2, Name = "TestRoom" },
                UserId = 3,
                User = new User { Id = 3, Username = "TestUser" },
                Info = "TestInfo"
            };

            // Act
            var result = RoomAccessLogMapper.toDTO(log);

            // Assert
            Assert.IsType<RoomAccessLogDTO>(result);
            Assert.Equal(log.Id, result.Id);
            Assert.Equal(log.RoomId, result.RoomId);
            Assert.Equal(log.Room.Name, result.RoomName);
            Assert.Equal(log.UserId, result.UserId);
            Assert.Equal(log.User.Username, result.Username);
            Assert.Equal(log.Info, result.Info);
        }

        #endregion

        #region ToDAL

        [Fact]
        public void ToDAL_WhenGivenRoomAccessLogDTO_ShouldReturnRoomAccessLog()
        {
            // Arrange
            RoomAccessLogDTO logDTO = new RoomAccessLogDTO
            {
                Id = 1,
                RoomId = 2,
                UserId = 3,
                Info = "TestInfo"
            };

            // Act
            var result = RoomAccessLogMapper.toDAL(logDTO);

            // Assert
            Assert.IsType<RoomAccessLog>(result);
            Assert.Equal(logDTO.Id, result.Id);
            Assert.Equal(logDTO.RoomId, result.RoomId);
            Assert.Equal(logDTO.UserId, result.UserId);
            Assert.Equal(logDTO.Info, result.Info);
        }

        #endregion
    }
}