using DAL.Enums;
using DAL.Models;
using Xunit;

namespace Tests.DAL
{
    public class AccessTest
    {
        [Fact]
        public void Access_DefaultValues_ShouldBeInitialized()
        {
            // Arrange
            var access = new Access();

            // Act & Assert
            Assert.Equal(0, access.RoomId);
            Assert.Null(access.Room);
            Assert.Equal(0, access.GroupId);
            Assert.Null(access.Group);
            Assert.Equal(default, access.AccessType);
        }

        [Fact]
        public void Access_SetRoomId_ShouldReturnCorrectValue()
        {
            // Arrange
            var access = new Access();
            var expectedRoomId = 1;

            // Act
            access.RoomId = expectedRoomId;

            // Assert
            Assert.Equal(expectedRoomId, access.RoomId);
        }

        [Fact]
        public void Access_SetRoom_ShouldReturnCorrectValue()
        {
            // Arrange
            var access = new Access();
            var expectedRoom = new Room { Id = 1, Name = "Test Room", RoomAbreviation = "TR" };

            // Act
            access.Room = expectedRoom;

            // Assert
            Assert.Equal(expectedRoom, access.Room);
        }

        [Fact]
        public void Access_SetGroupId_ShouldReturnCorrectValue()
        {
            // Arrange
            var access = new Access();
            var expectedGroupId = 1;

            // Act
            access.GroupId = expectedGroupId;

            // Assert
            Assert.Equal(expectedGroupId, access.GroupId);
        }

        [Fact]
        public void Access_SetGroup_ShouldReturnCorrectValue()
        {
            // Arrange
            var access = new Access();
            var expectedGroup = new Group { Id = 1, Name = "Test Group" };

            // Act
            access.Group = expectedGroup;

            // Assert
            Assert.Equal(expectedGroup, access.Group);
        }

        [Fact]
        public void Access_SetAccessType_ShouldReturnCorrectValue()
        {
            // Arrange
            var access = new Access();
            var expectedAccessType = AccessType.Allowed;

            // Act
            access.AccessType = expectedAccessType;

            // Assert
            Assert.Equal(expectedAccessType, access.AccessType);
        }
    }
}
