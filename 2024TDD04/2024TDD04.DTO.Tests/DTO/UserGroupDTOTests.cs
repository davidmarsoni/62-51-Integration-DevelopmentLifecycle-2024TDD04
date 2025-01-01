
using DTO;
using Xunit;

namespace _2024TDD04.DTO.Tests
{
    public class UserGroupDTOTests
    {
        [Fact]
        public void UserGroupDTO_WhenGivenValues_ShouldReturnInstantiatedUserGroupDTO()
        {
            // Arrange
            var expectedId = 1;
            var expectedUserId = 2;
            var expectedUsername = "Test User";
            var expectedGroupId = 3;
            var expectedGroupName = "Test Group";

            // Act
            var userGroupDTO = new UserGroupDTO
            {
                Id = expectedId,
                UserId = expectedUserId,
                Username = expectedUsername,
                GroupId = expectedGroupId,
                Groupname = expectedGroupName
            };

            // Assert
            Assert.Equal(expectedId, userGroupDTO.Id);
            Assert.Equal(expectedUserId, userGroupDTO.UserId);
            Assert.Equal(expectedUsername, userGroupDTO.Username);
            Assert.Equal(expectedGroupId, userGroupDTO.GroupId);
            Assert.Equal(expectedGroupName, userGroupDTO.Groupname);
        }
    }
}