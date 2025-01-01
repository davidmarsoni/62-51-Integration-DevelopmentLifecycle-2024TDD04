using DAL;
using DTO;
using WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using _2024TDD04.WebAPI.Tests.GeneralSetup;

namespace _2024TDD04.WebAPI.Tests.Controllers
{
    public class UserGroupsControllerTests {
        private readonly UserGroupsController _userGroupsController;
        private readonly RoomAccessContext _testDbContext;

        public UserGroupsControllerTests()
        {
            _testDbContext = InMemoryRoomContext.CreateInMemoryContext();
            _userGroupsController = new UserGroupsController(_testDbContext);
        }

        #region GetUsersInGroup
        [Fact]
        public async Task GetUsersInGroup_WhenGivenValidGroup_ReturnsListOfUsers()
        {
            // Arrange
            var groupId = 1;

            // Act
            var result = await _userGroupsController.GetUsersInGroup(groupId);

            // Assert
            Assert.IsType<ActionResult<IEnumerable<UserDTO>>>(result);
            Assert.NotEmpty(result.Value);
            Assert.Equal(1, result.Value.Count());
        }

        [Fact]
        public async Task GetUsersInGroup_WhenGivenNonExistingGroup_ReturnsNotFound()
        {
            // Arrange
            var nonExistentGroupId = 999;

            // Act
            var result = await _userGroupsController.GetUsersInGroup(nonExistentGroupId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetUsersInGroup_WhenGivenDeletedGroup_ReturnsNotFound()
        {
            // Arrange
            var groupId = 4;
                // Adding a usergroup to the database
            _testDbContext.UserGroups.Add(new UserGroup { Id = 4, UserId = 4, GroupId = 4 });
            _testDbContext.SaveChanges();
                // Deleting the group

            var group = _testDbContext.UserGroups.Find(4);
            _testDbContext.UserGroups.Remove(group);
            _testDbContext.SaveChanges();

            // Act
            var result = await _userGroupsController.GetUsersInGroup(groupId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        #endregion

        #region GetGroupsForUser
        [Fact]
        public async Task GetGroupsForUser_WhenGivenValidUser_ReturnsListOfGroups()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = await _userGroupsController.GetGroupsForUser(userId);

            // Assert
            Assert.IsType<ActionResult<IEnumerable<GroupDTO>>>(result);
            Assert.NotEmpty(result.Value);
            Assert.Equal(1, result.Value.Count());
        }

        [Fact]
        public async Task GetGroupsForUser_WhenGivenNonExistingUser_ReturnsNotFound()
        {
            // Arrange
            var nonExistentGroupId = 999;

            // Act
            var result = await _userGroupsController.GetGroupsForUser(nonExistentGroupId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetGroupsForUser_WhenGivenDeletedUser_ReturnsNotFound()
        {
            // Arrange
            var userId = 4;
            // Adding a usergroup to the database
            _testDbContext.UserGroups.Add(new UserGroup { Id = 4, UserId = userId, GroupId = 4 });
            _testDbContext.SaveChanges();
            // Deleting the group
            var group = _testDbContext.UserGroups.Find(userId);
            _testDbContext.UserGroups.Remove(group);
            _testDbContext.SaveChanges();

            // Act
            var result = await _userGroupsController.GetGroupsForUser(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        #endregion

        #region GetUserGroup
        [Fact]
        public async Task GetUserGroup_WhenGivenExistingUserGroup_ShouldReturnUserGroupDTO()
        {
            // Arrange
            var userGroupId = 1;

            // Act
            var result = await _userGroupsController.GetUserGroup(userGroupId);

            // Assert
            Assert.NotNull(result.Value);
            Assert.Equal(userGroupId, result.Value.Id);
        }

        [Fact]
        public async Task GetUserGroup_WhenGivenNonExistentUserGroup_ShouldReturnNotFound()
        {
            // Arrange
            var nonExistentId = 999;

            // Act
            var result = await _userGroupsController.GetUserGroup(nonExistentId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        #endregion

        #region PostUserGroup
        [Fact]
        public async Task PostUserGroup_WhenGivenValidData_ShouldReturnCreatedAtActionAndCreateUserGroup()
        {
            // Arrange
            var userGroupDTO = new UserGroupDTO { UserId = 2, GroupId = 1 };

            // Act
            var result = await _userGroupsController.PostUserGroup(userGroupDTO);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);

                // Check if the usergroup was created
            var userGroup = _testDbContext.UserGroups.Any(userGroup => userGroup.UserId == userGroupDTO.UserId && userGroup.GroupId == userGroupDTO.GroupId);
            Assert.NotNull(userGroup);
        }

        [Fact]
        public async Task PostUserGroup_WhenGivenUserAlreadyInGroup_ShouldReturnConflict()
        {
            // Arrange
            var userGroupDTO = new UserGroupDTO { UserId = 1, GroupId = 1 };

            // Act
            var result = await _userGroupsController.PostUserGroup(userGroupDTO);

            // Assert
            Assert.IsType<ConflictObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostUserGroup_WhenGivenDeletedUser_ShouldReturnForbid()
        {
            // Arrange
            var userGroupDTO = new UserGroupDTO { UserId = 3, GroupId = 1 };

            // Act
            var result = await _userGroupsController.PostUserGroup(userGroupDTO);

            // Assert
            Assert.IsType<ForbidResult>(result.Result);
        }

        [Fact]
        public async Task PostUserGroup_WhenGivenDeletedGroup_ShouldReturnForbid()
        {
            // Arrange
            var userGroupDTO = new UserGroupDTO { UserId = 1, GroupId = 5 };

            // Act
            var result = await _userGroupsController.PostUserGroup(userGroupDTO);

            // Assert
            Assert.IsType<ForbidResult>(result.Result);
        }
        #endregion

        #region DeleteUserFromGroup
        [Fact]
        public async Task DeleteUserFromGroup_WhenGivenValidData_ShouldReturnNoContentAndUpdateUserGroup()
        {
            // Arrange
            var existingUserId = 1;
            var existingGroupId = 1;

            // Act
            var result = await _userGroupsController.DeleteUserFromGroup(existingGroupId, existingUserId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUserFromGroup_WhenGivenNonExistantUser_ShouldReturnNotFound()
        {
            // Arrange
            var nonExistentUserId = 999;
            var existingGroupId = 1;

            // Act
            var result = await _userGroupsController.DeleteUserFromGroup(existingGroupId, nonExistentUserId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteUserFromGroup_WhenGivenNonExistantGroup_ShouldReturnNotFound()
        {
            // Arrange
            var existingUserId = 1;
            var nonExistentGroupId = 999;

            // Act
            var result = await _userGroupsController.DeleteUserFromGroup(existingUserId, nonExistentGroupId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion
    }
}