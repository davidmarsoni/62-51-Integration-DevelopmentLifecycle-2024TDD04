using DAL.Models;
using Xunit;

namespace _2024TDD04.DAL.Tests.Models
{
    public class RoomTest
    {

        [Fact]
        public void Room_SetId_ShouldReturnCorrectValue()
        {
            // Arrange
            var room = new Room { Id = 1, Name = "Test Room", RoomAbreviation = "TR" };
            var expectedId = 1;

            // Act
            room.Id = expectedId;

            // Assert
            Assert.Equal(expectedId, room.Id);
        }

        [Fact]
        public void Room_SetName_ShouldReturnCorrectValue()
        {
            // Arrange
            var room = new Room { Id = 1, Name = "Test Room", RoomAbreviation = "TR" };
            var expectedName = "Test Room";

            // Act
            room.Name = expectedName;

            // Assert
            Assert.Equal(expectedName, room.Name);
        }

        [Fact]
        public void Room_SetRoomAbreviation_ShouldReturnCorrectValue()
        {
            // Arrange
            var room = new Room { Id = 1, Name = "Test Room", RoomAbreviation = "TR" };
            var expectedRoomAbreviation = "TR";

            // Act
            room.RoomAbreviation = expectedRoomAbreviation;

            // Assert
            Assert.Equal(expectedRoomAbreviation, room.RoomAbreviation);
        }

        [Fact]
        public void Room_UserGroups_ShouldBeInitialized()
        {
            // Arrange
            var room = new Room { Id = 1, Name = "Test Room", RoomAbreviation = "TR" };

            // Act & Assert
            Assert.NotNull(room.User_Groups);
            Assert.Empty(room.User_Groups);
        }
    }
}
