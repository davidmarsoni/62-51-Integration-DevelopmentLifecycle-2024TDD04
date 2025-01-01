using DAL.Models;
using Xunit;
using Group = DAL.Models.Group;

namespace _2024TDD04.DAL.Tests.Models
{
    public class UserGroupTests
    {
        [Fact]
        public void UserGroup_WhenGivenValues_ShouldReturnInstantiatedUserGroup()
        {
            // Arrange
            var expectedUser = new User { Id = 1, Username = "Test User" };
            var expectedGroup = new Group { Id = 1, Name = "Test Group", Acronym = "TG" };

            var expectedId = 1;
            var expectedUserId = expectedUser.Id;
            var expectedGroupId = expectedGroup.Id;
            

            // Act
            var userGroup = new UserGroup
            {
                Id = expectedId,
                UserId = expectedUserId,
                User = expectedUser,
                GroupId = expectedGroupId,
                Group = expectedGroup
            };

            // Assert
            Assert.Equal(expectedId, userGroup.Id);
            Assert.Equal(expectedUserId, userGroup.UserId);
            Assert.Equal(expectedUser, userGroup.User);
            Assert.Equal(expectedGroupId, userGroup.GroupId);
            Assert.Equal(expectedGroup, userGroup.Group);
        }
    }
}
