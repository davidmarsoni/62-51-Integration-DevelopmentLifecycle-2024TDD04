using DAL.Models;
using Xunit;

namespace _2024TDD04.DAL.Tests.Models
{
    public class RoomTests
    {
        [Fact]
        public void Room_WhenGivenValues_ShouldReturnInstantiatedRoom()
        {
            // Arrange
            var expectedId = 1;
            var expectedName = "Test Room";
            var expectedAbreviation = "TR";
            var expectedIsDeleted = false;

            // Act
            var room = new Room
            {
                Id = expectedId,
                Name = expectedName,
                RoomAbreviation = expectedAbreviation,
                IsDeleted = expectedIsDeleted
            };

            // Assert
            Assert.Equal(expectedId, room.Id);
            Assert.Equal(expectedName, room.Name);
            Assert.Equal(expectedAbreviation, room.RoomAbreviation);
            Assert.Equal(expectedIsDeleted, room.IsDeleted);
            Assert.NotNull(room.User_Groups);
            Assert.Empty(room.User_Groups);
        }
    }
}
