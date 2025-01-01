
using DTO;
using Xunit;

namespace _2024TDD04.DTO.Tests
{
    public class RoomAccessDTOTests
    {
        [Fact]
        public void RoomAccessDTO_WhenGivenValues_ShouldReturnInstantiatedRoomAccessDTO()
        {
            // Arrange
            var expectedRoomId = 1;
            var expectedUserId = 2;

            // Act
            var roomAccessDTO = new RoomAccessDTO
            {
                RoomId = expectedRoomId,
                UserId = expectedUserId
            };

            // Assert
            Assert.Equal(expectedRoomId, roomAccessDTO.RoomId);
            Assert.Equal(expectedUserId, roomAccessDTO.UserId);
        }
    }
}