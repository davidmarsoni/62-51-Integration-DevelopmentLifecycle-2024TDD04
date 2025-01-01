
using DTO;
using Xunit;

namespace _2024TDD04.DTO.Tests
{
    public class AccessDTOTests
    {
        [Fact]
        public void AccessDTO_WhenGivenValues_ShouldReturnInstantiatedAccessDTO()
        {
            // Arrange
            var expectedId = 1;
            var expectedRoomId = 2;
            var expectedGroupId = 3;

            // Act
            var accessDTO = new AccessDTO
            {
                Id = expectedId,
                RoomId = expectedRoomId,
                GroupId = expectedGroupId
            };

            // Assert
            Assert.Equal(expectedId, accessDTO.Id);
            Assert.Equal(expectedRoomId, accessDTO.RoomId);
            Assert.Equal(expectedGroupId, accessDTO.GroupId);
        }
    }
}