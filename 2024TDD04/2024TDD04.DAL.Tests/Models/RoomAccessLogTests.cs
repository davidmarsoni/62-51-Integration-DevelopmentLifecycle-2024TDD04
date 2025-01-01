using DAL.Models;
using Xunit;
using Group = DAL.Models.Group;

namespace _2024TDD04.DAL.Tests.Models
{
    public class RoomAccessLogTests
    {
        [Fact]
        public void RoomAccessLog_WhenGivenValues_ShouldReturnInstanciatedRoomAccessLog(){
            // Arrange
            var expectedRoom = new Room { Id = 1, Name = "Test Room", RoomAbreviation = "TR" };
            var expectedUser = new User { Id = 1, Username = "Test User" };

            var expectedId = 1;
            var expectedRoomId = expectedRoom.Id;
            var expectedUserId = expectedUser.Id;
            var expectedAccessTime = System.DateTime.Now;
            var expectedInfo = "Test Info";

            // Act
            var roomAccessLog = new RoomAccessLog
            {
                Id = expectedId,
                RoomId = expectedRoomId,
                Room = expectedRoom,
                UserId = expectedUserId,
                User = expectedUser,
                Timestamp = expectedAccessTime,
                Info = expectedInfo
            };

            // Assert
            Assert.Equal(expectedId, roomAccessLog.Id);
            Assert.Equal(expectedRoomId, roomAccessLog.RoomId);
            Assert.Equal(expectedRoom, roomAccessLog.Room);
            Assert.Equal(expectedUserId, roomAccessLog.UserId);
            Assert.Equal(expectedUser, roomAccessLog.User);
            Assert.Equal(expectedAccessTime, roomAccessLog.Timestamp);
            Assert.Equal(expectedInfo, roomAccessLog.Info);
        }
    }
}
