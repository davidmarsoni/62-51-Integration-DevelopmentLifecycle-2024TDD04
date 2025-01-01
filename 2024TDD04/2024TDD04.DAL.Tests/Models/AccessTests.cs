using DAL.Models;
using Xunit;

namespace _2024TDD04.DAL.Tests.Models
{
    public class AccessTests
    {
        [Fact]
        public void Access_WhenGivenValues_ShouldReturnInstantiatedAccess()
        {
            // Arrange
            var expectedRoom = new Room { Id = 2, Name = "Test Room", RoomAbreviation = "TR" };
            var expectedGroup = new Group { Id = 3, Name = "Test Group", Acronym = "TG" };

            var expectedId = 1;
            var expectedRoomId = expectedRoom.Id;
            var expectedGroupId = expectedGroup.Id;

            // Act
            var access = new Access
            {
                Id = expectedId,
                RoomId = expectedRoomId,
                Room = expectedRoom,
                GroupId = expectedGroupId,
                Group = expectedGroup
            };

            // Assert
            Assert.Equal(expectedId, access.Id);
            Assert.Equal(expectedRoomId, access.RoomId);
            Assert.Equal(expectedRoom, access.Room);
            Assert.Equal(expectedGroupId, access.GroupId);
            Assert.Equal(expectedGroup, access.Group);
        }
    }
}
