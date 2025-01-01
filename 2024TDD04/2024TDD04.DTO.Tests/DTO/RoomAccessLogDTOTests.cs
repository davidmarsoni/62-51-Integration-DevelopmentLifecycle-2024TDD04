
using DTO;
using Xunit;

namespace _2024TDD04.DTO.Tests
{
    public class RoomAccessLogDTOTests
    {
        [Fact]
        public void RoomAccessLogDTO_WhenGivenValues_ShouldReturnInstantiatedRoomAccessLogDTO()
        {
            // Arrange
            var expectedId = 1;
            var expectedRoomId = 2;
            var expectedRoomName = "Test Room";
            var expectedUserId = 3;
            var expectedUsername = "Test User";
            var expectedTimestamp = System.DateTime.Now;
            var expectedInfo = "Test Info";

            // Act
            var roomAccessLogDTO = new RoomAccessLogDTO
            {
                Id = expectedId,
                RoomId = expectedRoomId,
                RoomName = expectedRoomName,
                UserId = expectedUserId,
                Username = expectedUsername,
                TimeStamp = expectedTimestamp,
                Info = expectedInfo
            };

            // Assert
            Assert.Equal(expectedId, roomAccessLogDTO.Id);
            Assert.Equal(expectedRoomId, roomAccessLogDTO.RoomId);
            Assert.Equal(expectedRoomName, roomAccessLogDTO.RoomName);
            Assert.Equal(expectedUserId, roomAccessLogDTO.UserId);
            Assert.Equal(expectedUsername, roomAccessLogDTO.Username);
            Assert.Equal(expectedTimestamp, roomAccessLogDTO.TimeStamp);
            Assert.Equal(expectedInfo, roomAccessLogDTO.Info);
        }
    }
}