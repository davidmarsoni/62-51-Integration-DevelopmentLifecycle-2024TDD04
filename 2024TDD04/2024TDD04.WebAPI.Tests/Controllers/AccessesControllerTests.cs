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

namespace _2024TDD04.DAL.Tests.WebAPI
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
        public async void HasAccessGroupAsync_CheckIfGroupHasAccess_ShouldReturnTrue(){
            // Arrange

            // Act
            // Teachers group has access to Room 301
            var result = await _accessesController.HasAccessGroupAsync(1, 1);

            // Assert
            Assert.True(result.Value);
        }

        [Fact]
        public async void HasAccessGroupAsync_CheckIfGroupHasAccess_ShouldReturnFalse(){
            // Arrange

            // Act
            // Students group does not have access to Room 301
            var result = await _accessesController.HasAccessGroupAsync(1, 2);

            // Assert
            Assert.False(result.Value);
        }

        [Fact]
        public async void HasAccessGroupAsync_CheckIfRoomDoesNotExist_ShouldReturnNotFound(){
            // Arrange

            // Act
            // Room Id 100 does not exist
            var result = await _accessesController.HasAccessGroupAsync(100, 1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void HasAccessGroupAsync_CheckIfGroupDoesNotExist_ShouldReturnNotFound(){
            // Arrange

            // Act
            // Group Id 100 does not exist
            var result = await _accessesController.HasAccessGroupAsync(1, 100);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        #endregion

        #region HasAccessUserAsync

        [Fact]     
        public async void HasAccessUserAsync_CheckIfUserHasAccess_ShouldReturnTrue(){
            // Arrange

            // Act
            // Mathias is in the Teachers group, which has access to Room 301
            var result = await _accessesController.HasAccessUserAsync(1, 1);

            // Assert
            Assert.True(result.Value);
        }  

        [Fact]
        public async void HasAccessUserAsync_CheckIfUserDoesNotHaveAccess_ShouldReturnFalse(){
            // Arrange

            // Act
            // David is in the Students group, which does not have access to Room 301
            var result = await _accessesController.HasAccessUserAsync(1, 2);

            // Assert
            Assert.False(result.Value);
        }

        [Fact]
        public async void HasAccessUserAsync_CheckIfUserIsNotInAnyGroup_ShouldReturnFalse(){
            // Arrange

            // Act
            // User Id 4 is not in any group
            var result = await _accessesController.HasAccessUserAsync(1, 4);

            // Assert
            Assert.False(result.Value);
        }

        [Fact]
        public async void HasAccessUserAsync_CheckIfRoomDoesNotExist_ShouldReturnNotFound(){
            // Arrange

            // Act
            // Room Id 100 does not exist
            var result = await _accessesController.HasAccessUserAsync(100, 1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);   
        }

        [Fact]
        public async void HasAccessUserAsync_CheckIfUserDoesNotExist_ShouldReturnNotFound(){
            // Arrange

            // Act
            // User Id 100 does not exist
            var result = await _accessesController.HasAccessUserAsync(1, 100);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        #endregion  

        #region GrantAccessAsync

        [Fact]
        public async void GrantAccessAsync_GrantAccess_ShouldReturnTrue(){
            // Arrange
            // Students group will be granted access to Classroom 301
            AccessDTO accessDTO = new AccessDTO { RoomId = 1, GroupId = 2 };

            // Act
            var result = await _accessesController.GrantAccessAsync(accessDTO);

            // Assert
            Assert.True(result.Value);
        }

        [Fact]
        public async void GrantAccessAsync_GrantAccessToRoomThatDoesNotExist_ShouldReturnNotFound(){
            // Arrange
            // Room Id 100 does not exist
            AccessDTO accessDTO = new AccessDTO { RoomId = 100, GroupId = 1 };

            // Act
            var result = await _accessesController.GrantAccessAsync(accessDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void GrantAccessAsync_GrantAccessToGroupThatDoesNotExist_ShouldReturnNotFound(){
            // Arrange
            // Group Id 100 does not exist
            AccessDTO accessDTO = new AccessDTO { RoomId = 3, GroupId = 100 };

            // Act
            var result = await _accessesController.GrantAccessAsync(accessDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void GrantAccessAsync_GrantAccessThatAlreadyExists_ShouldReturnConflict(){
            // Arrange
            // Teachers group already has already access to Room 301
            AccessDTO accessDTO = new AccessDTO { RoomId = 1, GroupId = 1 };

            // Act
            var result = await _accessesController.GrantAccessAsync(accessDTO);

            // Assert
            Assert.IsType<ConflictResult>(result.Result);
        }

        [Fact]
        public async void GrantAccessAsync_GrantAccessToDeletedRoom_ShouldReturnNotFound(){
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
        public async void RevokeAccessAsync_RoomOrGroupDoesNotExist_ShouldReturnNotFound(){
            // Arrange
            var dto = new AccessDTO { RoomId = 999, GroupId = 999 };

            // Act
            var result = await _accessesController.RevokeAccessAsync(dto);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void RevokeAccessAsync_AccessNotFound_ShouldReturnNotFound(){
            // Arrange
            var dto = new AccessDTO { RoomId = 1, GroupId = 2 }; 

            // Act
            var result = await _accessesController.RevokeAccessAsync(dto);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void RevokeAccessAsync_AccessFound_ShouldReturnTrue(){
            // Arrange
            var dto = new AccessDTO { RoomId = 1, GroupId = 1 };

            // Act
            var result = await _accessesController.RevokeAccessAsync(dto);

            // Assert
            Assert.True(result.Value);
        }

        [Fact]
        public async void RevokeAccessAsync_RevokeAccessToDeletedRoom_ShouldReturnNotFound(){
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
        public async void GetAccessesByUserIdAsync_UserDoesNotExist_ShouldReturnNotFound(){
            var result = await _accessesController.GetAccessesByUserId(100);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void GetAccessesByUserIdAsync_UserHasNoAccess_ShouldReturnNoContent(){
            // User 4 is not in any group
            var result = await _accessesController.GetAccessesByUserId(4);
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async void GetAccessesByUserIdAsync_UserHasAccess_ShouldReturnRooms(){
            // User 1 is in the Teachers group and has access to Room 301
            var result = await _accessesController.GetAccessesByUserId(1);
            Assert.NotNull(result.Value);
            Assert.NotEmpty(result.Value);
        }

        #endregion

        
    }
}
