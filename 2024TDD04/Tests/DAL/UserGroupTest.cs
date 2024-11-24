using DAL.Models;
using Group = DAL.Models.Group;

namespace Tests
{
    public class UserGroupTests
    {
        [Fact]
        public void UserGroup_DefaultValues_ShouldBeInitialized()
        {
            // Arrange
            var userGroup = new UserGroup();

            // Act & Assert
            Assert.Equal(0, userGroup.UserId);
            Assert.Null(userGroup.User);
            Assert.Equal(0, userGroup.GroupId);
            Assert.Null(userGroup.Group);
        }

        [Fact]
        public void UserGroup_SetUserId_ShouldReturnCorrectValue()
        {
            // Arrange
            var userGroup = new UserGroup();
            var expectedUserId = 1;

            // Act
            userGroup.UserId = expectedUserId;

            // Assert
            Assert.Equal(expectedUserId, userGroup.UserId);
        }

        [Fact]
        public void UserGroup_SetUser_ShouldReturnCorrectValue()
        {
            // Arrange
            var userGroup = new UserGroup();
            var expectedUser = new User { Id = 1, Username = "Test User" };

            // Act
            userGroup.User = expectedUser;

            // Assert
            Assert.Equal(expectedUser, userGroup.User);
        }

        [Fact]
        public void UserGroup_SetGroupId_ShouldReturnCorrectValue()
        {
            // Arrange
            var userGroup = new UserGroup();
            var expectedGroupId = 1;

            // Act
            userGroup.GroupId = expectedGroupId;

            // Assert
            Assert.Equal(expectedGroupId, userGroup.GroupId);
        }

        [Fact]
        public void UserGroup_SetGroup_ShouldReturnCorrectValue()
        {
            // Arrange
            var userGroup = new UserGroup();
            var expectedGroup = new Group { Id = 1, Name = "Test Group", Acronym = "TG" };

            // Act
            userGroup.Group = expectedGroup;

            // Assert
            Assert.Equal(expectedGroup, userGroup.Group);
        }
    }
}
