
using DTO;
using Xunit;

namespace _2024TDD04.DTO.Tests
{
    public class RoomDTOTests
    {
        [Fact]
        public void RoomDTO_WhenGivenValues_ShouldReturnInstantiatedRoomDTO()
        {
            // Arrange
            var expectedId = 1;
            var expectedName = "Test Room";
            var expectedAbreviation = "TR";
            var expectedIsDeleted = false;

            // Act
            var roomDTO = new RoomDTO
            {
                Id = expectedId,
                Name = expectedName,
                RoomAbreviation = expectedAbreviation,
                IsDeleted = expectedIsDeleted
            };

            // Assert
            Assert.Equal(expectedId, roomDTO.Id);
            Assert.Equal(expectedName, roomDTO.Name);
            Assert.Equal(expectedAbreviation, roomDTO.RoomAbreviation);
            Assert.Equal(expectedIsDeleted, roomDTO.IsDeleted);
        }
    }
}