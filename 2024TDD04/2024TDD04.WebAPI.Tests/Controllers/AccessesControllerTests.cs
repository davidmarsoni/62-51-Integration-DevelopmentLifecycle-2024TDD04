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
        public async void HasAccessGroupAsync_WhenGivenGroupWithAccess_ShouldReturnTrue(){
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
        public async void HasAccessGroupAsync_WhenGivenGroupWithoutAccess_ShouldReturnFalse(){
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
        public async void HasAccessGroupAsync_WhenGivenNonExistantRoom_ShouldReturnNotFound(){
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
        public async void HasAccessGroupAsync_WhenGivenNonExistantGroup_ShouldReturnNotFound(){
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
        public async void HasAccessUserAsync_WhenGivenUserWithAccess_ShouldReturnTrue(){
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
        public async void HasAccessUserAsync_WhenGivenUserWithoutAccess_ShouldReturnFalse(){
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
        public async void HasAccessUserAsync_WhenGivenUserWithoutGroup_ShouldReturnFalse(){
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
        public async void HasAccessUserAsync_WhenGivenNonExistantUser_ShouldReturnNotFound(){
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
        public async void HasAccessUserAsync_WhenGivenNonExistantRoom_ShouldReturnNotFound(){
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
        public async void GrantAccessAsync_WhenGivenValidData_ShouldReturnTrueAndGrantAccess(){
            // Arrange
            // Students group will be granted access to Classroom 301
            AccessDTO accessDTO = new AccessDTO { RoomId = 1, GroupId = 2 };

            // Act
            var result = await _accessesController.GrantAccessAsync(accessDTO);

            // Assert
            Assert.True(result.Value);
        }

        [Fact]
        public async void GrantAccessAsync_WhenGivenNonExistantRoom_ShouldReturnNotFound(){
            // Arrange
            // Room Id 100 does not exist
            AccessDTO accessDTO = new AccessDTO { RoomId = 100, GroupId = 1 };

            // Act
            var result = await _accessesController.GrantAccessAsync(accessDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void GrantAccessAsync_WhenGivenNonExistantGroup_ShouldReturnNotFound(){
            // Arrange
            // Group Id 100 does not exist
            AccessDTO accessDTO = new AccessDTO { RoomId = 3, GroupId = 100 };

            // Act
            var result = await _accessesController.GrantAccessAsync(accessDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void GrantAccessAsync_WhenGivenExistingGrantedAccess_ShouldReturnConflict(){
            // Arrange
            // Teachers group already has already access to Room 301
            AccessDTO accessDTO = new AccessDTO { RoomId = 1, GroupId = 1 };

            // Act
            var result = await _accessesController.GrantAccessAsync(accessDTO);

            // Assert
            Assert.IsType<ConflictResult>(result.Result);
        }

        [Fact]
        public async void GrantAccessAsync_WhenGivenDeletedRoom_ShouldReturnNotFound(){
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
        public async void RevokeAccessAsync_WhenGivenValidEntry_ShouldReturnTrueAndRevokeAccess(){
            // Arrange
            var dto = new AccessDTO { RoomId = 1, GroupId = 1 };

            // Act
            var result = await _accessesController.RevokeAccessAsync(dto);

            // Assert
            Assert.True(result.Value);
        }

        [Fact]
        public async void RevokeAccessAsync_WhenGivenNonExistantRoom_ShouldReturnNotFound(){
            // Arrange
            var dto = new AccessDTO { RoomId = 999, GroupId = 1 };

            // Act
            var result = await _accessesController.RevokeAccessAsync(dto);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void RevokeAccessAsync_WhenGivenNonExistantGroup_ShouldReturnNotFound(){
            // Arrange
            var dto = new AccessDTO { RoomId = 1, GroupId = 999 };

            // Act
            var result = await _accessesController.RevokeAccessAsync(dto);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void RevokeAccessAsync_WhenGivenDeletedRoom_ShouldReturnNotFound(){
            // Arrange
            var dto = new AccessDTO { RoomId = 3, GroupId = 1 };

            // Act
            var result = await _accessesController.RevokeAccessAsync(dto);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        #endregion

        #region GetAccessesByUserId

        [Fact]
        public async void GetAccessesByUserIdAsync_WhenGivenUserWithAccess_ShouldReturnListOfRooms(){
            // Arrange

            // Act
            var result = await _accessesController.GetAccessesByUserId(1);

            // Assert
            Assert.NotNull(result.Value);
            Assert.NotEmpty(result.Value);
        }

        [Fact]
        public async void GetAccessesByUserIdAsync_WhenGivenUserWithoutAccess_ShouldReturnNoContent(){
            // Arrange

            // Act
            var result = await _accessesController.GetAccessesByUserId(4);

            // Assert
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async void GetAccessesByUserIdAsync_WhenGivenNonExistantUser_ShouldReturnNotFound(){
            // Arrange

            // Act
            var result = await _accessesController.GetAccessesByUserId(1);

            // Assert
            Assert.NotNull(result.Value);
            Assert.NotEmpty(result.Value);
        }

        #endregion

        
    }
}
