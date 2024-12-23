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

        #region HasAccessAsync

        [Fact]
        public async void HasAccessUserAsync_CheckIfGroupHasAccess_ShouldReturnTrue(){
            // Arrange

            // Act
            // Teachers group has access to Room 301
            var result = await _accessesController.HasAccessAsync(1, 1);

            // Assert
            Assert.True(result.Value);
        }

        [Fact]
        public async void HasAccessUserAsync_CheckIfGroupHasAccess_ShouldReturnFalse(){
            // Arrange

            // Act
            // Students group does not have access to Room 301
            var result = await _accessesController.HasAccessAsync(1, 2);

            // Assert
            Assert.False(result.Value);
        }

        [Fact]
        public async void HasAccessAsync_CheckIfRoomDoesNotExist_ShouldReturnNotFound(){
            // Arrange

            // Act
            // Room Id 100 does not exist
            var result = await _accessesController.HasAccessAsync(100, 1);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void HasAccessUserAsync_CheckIfGroupDoesNotExist_ShouldReturnNotFound(){
            // Arrange

            // Act
            // Group Id 100 does not exist
            var result = await _accessesController.HasAccessAsync(1, 100);

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
            // Teachers group will be granted access to Utility Closet R303
            AccessDTO accessDTO = new AccessDTO { RoomId = 3, GroupId = 1 };

            // Act
            var result = await _accessesController.GrantAccessAsync(accessDTO);

            // Assert
            Assert.True(result.Value);
        }

        [Fact]
        public async void GrantAccessAsync_GrantAccessToRoomThatDoesNotExist_ShouldReturnBadRequest(){
            // Arrange
            // Room Id 100 does not exist
            AccessDTO accessDTO = new AccessDTO { RoomId = 100, GroupId = 1 };

            // Act
            var result = await _accessesController.GrantAccessAsync(accessDTO);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async void GrantAccessAsync_GrantAccessToGroupThatDoesNotExist_ShouldReturnBadRequest(){
            // Arrange
            // Group Id 100 does not exist
            AccessDTO accessDTO = new AccessDTO { RoomId = 3, GroupId = 100 };

            // Act
            var result = await _accessesController.GrantAccessAsync(accessDTO);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
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

        #endregion

        #region GetRoomAccessedByGroupAsync

        [Fact]
        public async void GetRoomAccessedByGroupAsync_GroupDoesNotExist_ShouldReturnNotFound(){
            // Arrange

            // Act
            var result = await _accessesController.GetRoomAccessedByGroupAsync(999); 

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void GetRoomAccessedByGroupAsync_NoRoomForGroup_ShouldReturnNotFound(){
            // Arrange

            // Act
            var result = await _accessesController.GetRoomAccessedByGroupAsync(3);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void GetRoomAccessedByGroupAsync_GroupHasAccess_ShouldReturnRoom(){
            // Arrange

            // Act
            var result = await _accessesController.GetRoomAccessedByGroupAsync(1);

            // Assert
            Assert.IsType<RoomDTO>(result.Value);
        }

        #endregion

        #region GetRoomAccessedByUserAsync

        [Fact]
        public async void GetRoomAccessedByUserAsync_UserDoesNotExist_ShouldReturnBadRequest(){
            // Arrange

            // Act
            var result = await _accessesController.GetRoomAccessedByUserAsync(999);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async void GetRoomAccessedByUserAsync_UserInNoGroup_ShouldReturnNotFound(){
            // Arrange

            // Act
            var result = await _accessesController.GetRoomAccessedByUserAsync(3);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void GetRoomAccessedByUserAsync_NoRoomForUserGroup_ShouldReturnNotFound(){
            // Arrange

            // Act
            var result = await _accessesController.GetRoomAccessedByUserAsync(3);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void GetRoomAccessedByUserAsync_UserHasNoGroup_ShouldReturnNotFound(){
            // Arrange

            // Act
            var result = await _accessesController.GetRoomAccessedByUserAsync(4);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async void GetRoomAccessedByUserAsync_UserHasAccess_ShouldReturnRoom(){
            // Arrange

            // Act
            var result = await _accessesController.GetRoomAccessedByUserAsync(2);

            // Assert
            Assert.IsType<RoomDTO>(result.Value);
        }

        #endregion
    }
}
