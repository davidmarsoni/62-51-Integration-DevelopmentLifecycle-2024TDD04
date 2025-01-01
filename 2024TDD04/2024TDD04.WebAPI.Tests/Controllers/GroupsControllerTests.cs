
using DAL;
using DTO;
using WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using WebApi.Mapper;
using _2024TDD04.WebAPI.Tests.GeneralSetup;
using DAL.Models;

namespace _2024TDD04.WebAPI.Tests.Controllers
{
    public class GroupsControllerTests {
        private readonly GroupsController _groupsController;
        private readonly RoomAccessContext _testDbContext;

        public GroupsControllerTests()
        {
            _testDbContext = InMemoryRoomContext.CreateInMemoryContext();
            _groupsController = new GroupsController(_testDbContext);
        }


        #region GetGroups

        [Fact]
        public async Task GetGroups_WhenGroupsInDB_ShouldReturnListOfGroups()
        {
            // Arrange
        
            // Act
            var result = await _groupsController.GetGroups();
        
            // Then
            Assert.IsType<ActionResult<IEnumerable<GroupDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(5, result.Value.Count());
        }

        [Fact]
        public async Task GetGroups_WhenNoGroupsInDB_ShouldReturnEmptyList()
        {
            // Arrange
            _testDbContext.Groups.RemoveRange(_testDbContext.Groups);
            _testDbContext.SaveChanges();
        
            // Act
            var result = await _groupsController.GetGroups();
        
            // Then
            Assert.IsType<ActionResult<IEnumerable<GroupDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
        }   

        #endregion

        #region GetGroupsActive

        [Fact]
        public async Task GetGroupsActive_WhenActiveGroupsInDB_ShouldReturnListOfActiveGroups()
        {
            // Arrange
        
            // Act
            var result = await _groupsController.GetGroupsActive();
        
            // Then
            Assert.IsType<ActionResult<IEnumerable<GroupDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(4, result.Value.Count());

            foreach (var group in result.Value)
            {
                Assert.False(group.IsDeleted);
            }
        }

        [Fact]
        public async Task GetGroupsActive_WhenNoActiveGroupsInDB_ShouldReturnEmptyList()
        {
            // Arrange
            _testDbContext.Groups.RemoveRange(_testDbContext.Groups);
            _testDbContext.SaveChanges();
        
            // Act
            var result = await _groupsController.GetGroupsActive();
        
            // Then
            Assert.IsType<ActionResult<IEnumerable<GroupDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
        }

        #endregion

        #region GetGroup

        [Fact]
        public async Task GetGroup_WhenGivenValidGroup_ShouldReturnGroup()
        {
            // Arrange
            var groupId = 1;
        
            // Act
            var result = await _groupsController.GetGroup(groupId);
        
            // Then
            Assert.IsType<ActionResult<GroupDTO>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(groupId, result.Value.Id);
        }

        [Fact]
        public async Task GetGroup_WhenGivenNonExistentGroup_ShouldReturnNotFound()
        {
            // Arrange
            var groupId = 99;
        
            // Act
            var result = await _groupsController.GetGroup(groupId);
        
            // Then
            Assert.IsType<ActionResult<GroupDTO>>(result);
            Assert.Null(result.Value);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetGroup_WhenGivenDeletedGroup_ShouldReturnForbid()
        {
            // Arrange
            var groupId = 5;
        
            // Act
            var result = await _groupsController.GetGroup(groupId);
        
            // Then
            Assert.IsType<ActionResult<GroupDTO>>(result);
            Assert.Null(result.Value);
            Assert.IsType<ForbidResult>(result.Result);
        }

        #endregion

        #region PostGroup

        [Fact]
        public async Task PostGroup_WhenGivenValidGroup_ShouldReturnCreatedAtActionAndCreateGroup()
        {
            // Arrange
            var group = new GroupDTO { Name = "New Group", Acronym = "NG" };
        
            // Act
            var result = await _groupsController.PostGroup(group);
        
            // Then
            Assert.IsType<ActionResult<GroupDTO>>(result);
            Assert.IsType<CreatedAtActionResult>(result.Result);

                // Check if the group was added to the database
            var groups = await _groupsController.GetGroups();
            var lastGroup = groups.Value.Last();
            Assert.Equal(group.Name, lastGroup.Name);
        }

        [Fact]
        public async Task PostGroup_WhenGivenExistingGroup_ShouldReturnConflict()
        {
            // Arrange
            var group = new GroupDTO { Name = "Teachers", Acronym = "TCH" };
        
            // Act
            var result = await _groupsController.PostGroup(group);
        
            // Then
            Assert.IsType<ActionResult<GroupDTO>>(result);
            Assert.Null(result.Value);
            Assert.IsType<ConflictObjectResult>(result.Result);
        }

        #endregion

        #region PutGroup

        [Fact]
        public async Task PutGroup_WhenGivenValidGroup_ShouldReturnNoContentAndUpdateGroup()
        {
            // Arrange
            var groupId = 1;
            var group = new GroupDTO { Id = groupId, Name = "Updated Group", Acronym = "UG" };
        
            // Act
            var result = await _groupsController.PutGroup(groupId, group);
        
            // Then
            Assert.IsType<NoContentResult>(result);

                // Check if the group was updated in the database
            var updatedGroup = await _testDbContext.Groups.FindAsync(groupId);
            Assert.Equal(group.Name, updatedGroup.Name);
        }

        [Fact]
        public async Task PutGroup_WhenGivenNonMatchingData_ShouldReturnBadRequest()
        {
            // Arrange
            var groupId = 1;
            var group = new GroupDTO { Id = 2, Name = "Updated Group", Acronym = "UG" };
        
            // Act
            var result = await _groupsController.PutGroup(groupId, group);
        
            // Then
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PutGroup_WhenGivenNonExistentGroup_ShouldReturnNotFound()
        {
            // Arrange
            var groupId = 99;
            var group = new GroupDTO { Id = groupId, Name = "Updated Group", Acronym = "UG" };
        
            // Act
            var result = await _groupsController.PutGroup(groupId, group);
        
            // Then
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion

        #region DeleteGroup

        [Fact]
        public async Task DeleteGroup_WhenGivenExistingGroup_ShouldReturnNoContentAndUpdateGroup()
        {
            // Arrange
            var groupId = 1;
        
            // Act
            var result = await _groupsController.DeleteGroup(groupId);
        
            // Then
            Assert.IsType<NoContentResult>(result);

                // Check if the group was deleted in the database
            var deletedGroup = await _testDbContext.Groups.FindAsync(groupId);
            Assert.True(deletedGroup.IsDeleted);
        }


        [Fact]
        public async Task DeleteGroup_WhenGivenNonExistentGroup_ShouldReturnNotFound()
        {
            // Arrange
            var groupId = 99;
        
            // Act
            var result = await _groupsController.DeleteGroup(groupId);
        
            // Then
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion

        #region GroupNameExists

        [Fact]
        public async Task GroupNameExists_WhenGivenExistingGroupName_ShouldReturnTrue()
        {
            // Arrange
            var groupName = "Teachers";
        
            // Act
            var result = await _groupsController.GroupNameExists(groupName);
        
            // Then
            Assert.IsType<ActionResult<bool>>(result);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task GroupNameExists_WhenGivenNonExistentGroupName_ShouldReturnFalse()
        {
            // Arrange
            var groupName = "NonExistentGroup";
        
            // Act
            var result = await _groupsController.GroupNameExists(groupName);
        
            // Then
            Assert.IsType<ActionResult<bool>>(result);
            Assert.False(result.Value);
        }

        #endregion

        #region GroupAcronymExists

        [Fact]
        public async Task GroupAcronymExists_WhenGivenExistingGroupAcronym_ShouldReturnTrue()
        {
            // Arrange
            var groupAcronym = "TCH";
        
            // Act
            var result = await _groupsController.GroupAcronymExists(groupAcronym);
        
            // Then
            Assert.IsType<ActionResult<bool>>(result);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task GroupAcronymExists_WhenGivenNonExistentGroupAcronym_ShouldReturnFalse()
        {
            // Arrange
            var groupAcronym = "NonExistentAcronym";
        
            // Act
            var result = await _groupsController.GroupAcronymExists(groupAcronym);
        
            // Then
            Assert.IsType<ActionResult<bool>>(result);
            Assert.False(result.Value);
        }

        #endregion

    }
}