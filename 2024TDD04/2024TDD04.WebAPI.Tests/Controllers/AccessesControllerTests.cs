using DAL;
using DAL.Models;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using WebAPI.Controllers;
using Xunit;
using _2024TDD04.WebAPI.Tests.GeneralSetup;

namespace _2024TDD04.WebAPI.Tests.Controllers
{
    public class AccessesControllerTests
    {
        private readonly AccessesController _accessesController;
        private readonly RoomAccessContext _testDbContext;

        public AccessesControllerTests()
        {
            _testDbContext = InMemoryRoomContext.CreateInMemoryContext();
            _accessesController = new AccessesController(_testDbContext);
        }

        #region HasAccessGroupAsync

        [Fact]
        public async Task HasAccessGroupAsync_WhenGivenGroupWithAccess_ShouldReturnTrue(){
            // Arrange
            // Teachers group has access to Room 301
            var room = 1;
            var group = 1;

            // Act
            var result = await _accessesController.HasAccessGroupAsync(room, group);

            // Assert
            Assert.True(result.Value);
        }

        [Fact]
        public async Task HasAccessGroupAsync_WhenGivenGroupWithoutAccess_ShouldReturnFalse(){
            // Arrange
            // Students group does not have access to Room 301
            var room = 1;
            var group = 2;

            // Act
            var result = await _accessesController.HasAccessGroupAsync(room, group);

            // Assert
            Assert.False(result.Value);
        }

        [Fact]
        public async Task HasAccessGroupAsync_WhenGivenNonExistentRoom_ShouldReturnNotFound(){
            // Arrange
            // Room Id 100 does not exist
            var room = 100;
            var group = 1;

            // Act
            var result = await _accessesController.HasAccessGroupAsync(room, group);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task HasAccessGroupAsync_WhenGivenNonExistentGroup_ShouldReturnNotFound(){
            // Arrange
            // Group Id 100 does not exist
            var room = 1;
            var group = 100;

            // Act
            var result = await _accessesController.HasAccessGroupAsync(room, group);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        #endregion

        #region HasAccessUserAsync

        [Fact]     
        public async Task HasAccessUserAsync_WhenGivenUserWithAccess_ShouldReturnTrue(){
            // Arrange
            // Mathias is in the Teachers group, which has access to Room 301
            var room = 1;
            var user = 1;

            // Act
            var result = await _accessesController.HasAccessUserAsync(room, user);

            // Assert
            Assert.True(result.Value);
        }  

        [Fact]
        public async Task HasAccessUserAsync_WhenGivenUserWithoutAccess_ShouldReturnFalse(){
            // Arrange
            // David is in the Students group, which does not have access to Room 301
            var room = 1;
            var user = 2;

            // Act
            var result = await _accessesController.HasAccessUserAsync(room, user);

            // Assert
            Assert.False(result.Value);
        }

        [Fact]
        public async Task HasAccessUserAsync_WhenGivenUserWithoutGroup_ShouldReturnFalse(){
            // Arrange
            // User Id 4 is not in any group
            var room = 1;
            var user = 4;

            // Act
            var result = await _accessesController.HasAccessUserAsync(room, user);

            // Assert
            Assert.False(result.Value);
        }

        [Fact]
        public async Task HasAccessUserAsync_WhenGivenNonExistentUser_ShouldReturnNotFound(){
            // Arrange
            // User Id 100 does not exist
            var room = 1;
            var user = 100;

            // Act
            var result = await _accessesController.HasAccessUserAsync(room, user);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

         [Fact]
        public async Task HasAccessUserAsync_WhenGivenNonExistentRoom_ShouldReturnNotFound(){
            // Arrange
            // Room Id 100 does not exist
            var room = 100;
            var user = 1;

            // Act
            var result = await _accessesController.HasAccessUserAsync(room, user);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);   
        }

        #endregion  

        #region GrantAccessAsync

        [Fact]
        public async Task GrantAccessAsync_WhenGivenValidData_ShouldReturnTrueAndGrantAccess(){
            // Arrange
            // Students group will be granted access to Classroom 301
            AccessDTO accessDTO = new AccessDTO { RoomId = 1, GroupId = 2 };

            // Act
            var result = await _accessesController.GrantAccessAsync(accessDTO);

            // Assert
            Assert.True(result.Value);
        }

        [Fact]
        public async Task GrantAccessAsync_WhenGivenNonExistentRoom_ShouldReturnNotFound(){
            // Arrange
            // Room Id 100 does not exist
            AccessDTO accessDTO = new AccessDTO { RoomId = 100, GroupId = 1 };

            // Act
            var result = await _accessesController.GrantAccessAsync(accessDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GrantAccessAsync_WhenGivenNonExistentGroup_ShouldReturnNotFound(){
            // Arrange
            // Group Id 100 does not exist
            AccessDTO accessDTO = new AccessDTO { RoomId = 3, GroupId = 100 };

            // Act
            var result = await _accessesController.GrantAccessAsync(accessDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GrantAccessAsync_WhenGivenExistingGrantedAccess_ShouldReturnConflict(){
            // Arrange
            // Teachers group already has already access to Room 301
            AccessDTO accessDTO = new AccessDTO { RoomId = 1, GroupId = 1 };

            // Act
            var result = await _accessesController.GrantAccessAsync(accessDTO);

            // Assert
            Assert.IsType<ConflictResult>(result.Result);
        }

        [Fact]
        public async Task GrantAccessAsync_WhenGivenDeletedRoom_ShouldReturnNotFound(){
            // Arrange
            // Room 303 is deleted
            AccessDTO accessDTO = new AccessDTO { RoomId = 3, GroupId = 1 };

            // Act
            var result = await _accessesController.GrantAccessAsync(accessDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        #endregion

        #region RevokeAccessAsync

        [Fact]
        public async Task RevokeAccessAsync_WhenGivenValidEntry_ShouldReturnTrueAndRevokeAccess(){
            // Arrange
            var roomId = 1;
            var groupId = 1;

            // Act
            var result = await _accessesController.RevokeAccessAsync(roomId, groupId);

            // Assert
            Assert.True(result.Value);
        }

        [Fact]
        public async Task RevokeAccessAsync_WhenGivenNonExistentRoom_ShouldReturnNotFound(){
            // Arrange
            var roomId = 999;
            var groupId = 1;

            // Act
            var result = await _accessesController.RevokeAccessAsync(roomId, groupId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task RevokeAccessAsync_WhenGivenNonExistentGroup_ShouldReturnNotFound(){
            // Arrange
            var roomId = 1;
            var groupId = 999;

            // Act
            var result = await _accessesController.RevokeAccessAsync(roomId, groupId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task RevokeAccessAsync_WhenGivenDeletedRoom_ShouldReturnNotFound(){
            // Arrange
            var roomId = 3;
            var groupId = 1;

            // Act
            var result = await _accessesController.RevokeAccessAsync(roomId, groupId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        #endregion

        #region GetAccessesByUserId

        [Fact]
        public async Task GetAccessesByUserIdAsync_WhenGivenUserWithAccess_ShouldReturnListOfRooms(){
            // Arrange
            var userId = 1;

            // Act
            var result = await _accessesController.GetAccessesByUserId(userId);

            // Assert
            Assert.NotNull(result.Value);
            Assert.NotEmpty(result.Value);
        }

        [Fact]
        public async Task GetAccessesByUserIdAsync_WhenGivenUserWithoutAccess_ShouldReturnNoContent(){
            // Arrange
            var userId = 4;

            // Act
            var result = await _accessesController.GetAccessesByUserId(userId);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async Task GetAccessesByUserIdAsync_WhenGivenNonExistentUser_ShouldReturnNotFound(){
            // Arrange
            var userId = 999;

            // Act
            var result = await _accessesController.GetAccessesByUserId(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        #endregion

        #region GetAccessesByGroupId

        [Fact]
        public async Task GetAccessesByGroupIdAsync_WhenGivenGroupWithAccess_ShouldReturnRooms()
        {
            // Arrange
            var groupId = 1;

            // Act
            var result = await _accessesController.GetAccessesByGroupId(groupId);
            
            // Assert
            Assert.NotNull(result.Value);
            Assert.NotEmpty(result.Value);
        }

        [Fact]
        public async Task GetAccessesByGroupIdAsync_WhenGivenGroupWithoutAccess_ShouldReturnEmpty()
        {
            // Arrange
            var groupId = 4;

            // Act
            var result = await _accessesController.GetAccessesByGroupId(groupId);

            // Assert
            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task GetAccessesByGroupIdAsync_WhenGivenNonExistentGroup_ShouldReturnNotFound()
        {
            // Arrange
            var groupId = 999;

            // Act
            var result = await _accessesController.GetAccessesByGroupId(groupId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        #endregion

        
    }
}
