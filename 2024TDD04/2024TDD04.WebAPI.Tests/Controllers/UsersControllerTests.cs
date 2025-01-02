using DAL;
using DAL.Models;
using DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq;
using WebApi.Controllers;
using WebAPI.Controllers;
using Xunit;
using _2024TDD04.WebAPI.Tests.GeneralSetup;
using WebApi.Controllers.Interfaces;

namespace _2024TDD04.WebAPI.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly IUsersController _usersController;
        private readonly RoomAccessContext _testDbContext;

        public UsersControllerTests()
        {
            _testDbContext = InMemoryRoomContext.CreateInMemoryContext();
            _usersController = new UsersController(_testDbContext);
        }

        #region GetUsers

        [Fact]
        public async Task GetUsers_WhenUsersInDB_ShouldReturnListOfUsers()
        {
            // Arrange

            // Act
            var result = await _usersController.GetUsers();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<UserDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(4, result.Value.Count());
        }

        [Fact]
        public async Task GetUsers_WhenNoUsersInDB_ShouldReturnEmptyList()
        {
            // Arrange
            _testDbContext.Users.RemoveRange(_testDbContext.Users);
            _testDbContext.SaveChanges();

            // Act
            var result = await _usersController.GetUsers();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<UserDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
        }



        #endregion

        #region GetUsernameExist

        [Fact]
        public async Task UsernameExist_WhenGivenExistingUsername_ShouldReturnTrue()
        {
            // Arrange
            var existentUsername = "Widmer";

            // Act
            var result = await _usersController.UsernameExist(existentUsername);

            // Assert
            Assert.IsType<ActionResult<bool>>(result);
            Assert.True(result.Value);
        }

        [Fact]
        public async Task UsernameExist_WhenGivenNonExistentUsername_ShouldReturnFalse()
        {
            // Arrange
            var nonExistentUsername = "TestUsername";

            // Act
            var result = await _usersController.UsernameExist(nonExistentUsername);

            // Assert
            Assert.IsType<ActionResult<bool>>(result);
            Assert.False(result.Value);
        }

        #endregion

        #region GetUsersActive

        [Fact]
        public async Task GetUsersActive_WhenActiveUsersInDB_ShouldReturnListOfActiveUsers()
        {
            // Arrange

            // Act
            var result = await _usersController.GetUsersActive();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<UserDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(3, result.Value.Count());
        }

        [Fact]
        public async Task GetUsersActive_WhenNoActiveUsersInDB_ShouldReturnEmptyList()
        {
            // Arrange
            foreach (var user in _testDbContext.Users)
            {
                user.IsDeleted = true;
            }
            _testDbContext.SaveChanges();

            // Act
            var result = await _usersController.GetUsersActive();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<UserDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
        }

        #endregion

        #region GetUser

        [Fact]
        public async Task GetUser_WhenGivenExistingUser_ShouldReturnUserDTO()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = await _usersController.GetUser(userId);

            // Assert
            Assert.IsType<ActionResult<UserDTO>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
        }

        [Fact]
        public async Task GetUser_WhenGivenNonExistentUser_ShouldReturnNotFound()
        {
            // Arrange
            var nonExistentUserId = 999;

            // Act
            var result = await _usersController.GetUser(nonExistentUserId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        #endregion

        #region PutUser

        [Fact]
        public async Task PutUser_WhenGivenValidUser_ShouldReturnNoContentAndUpdateUser()
        {
            // Arrange
            var userId = 1;
            UserDTO userDTO = new UserDTO
            {
                Id = userId,
                Username = "TestUser",
                IsDeleted = false
            };

            // Act
            var result = await _usersController.PutUser(userId, userDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);

            User user = await _testDbContext.Users.FindAsync(1);
            Assert.NotNull(user);
            Assert.Equal(userDTO.Username, user.Username);
        }

        [Fact]
        public async Task PutUser_WhenGivenNonExistantUser_ShouldReturnNotFound()
        {
            // Arrange
            var nonExistentUserId = 999;
            UserDTO userDTO = new UserDTO
            {
                Id = nonExistentUserId,
                Username = "TestUser",
                IsDeleted = false
            };

            // Act
            var result = await _usersController.PutUser(nonExistentUserId, userDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutUser_WhenGivenNonMatchingData_ShouldReturnBadRequest()
        {
            // Arrange
            var userId = 1;
            var wrongUserId = 999;
            UserDTO userDTO = new UserDTO
            {
                Id = userId,
                Username = "TestUser",
                IsDeleted = false
            };
            
            // Act
            var result = await _usersController.PutUser(wrongUserId, userDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        #endregion

        #region PostUser

        [Fact]
        public async Task PostUser_WhenGivenValidUser_ShouldReturnCreatedAtActionAndCreateUser()
        {
            // Arrange
            UserDTO userDTO = new UserDTO
            {
                Username = "TestUser",
                IsDeleted = false
            };

            // Act
            var result = await _usersController.PostUser(userDTO);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task PostUser_WhenGivenExistingUser_ShouldReturnConflict()
        {
            // Arrange
            UserDTO userDTO = new UserDTO
            {
                Id = 1,
                Username = "TestUser",
                IsDeleted = false
            };

            // Act
            var result = await _usersController.PostUser(userDTO);

            // Assert
            Assert.IsType<ConflictObjectResult>(result.Result);
        }

        #endregion

        #region DeleteUser

        [Fact]
        public async Task DeleteUser_WhenGivenValidUser_ShouldReturnNoContentAndUpdateUser()
        {
            // Arrange
            var userId = 1;

            // Act
            var result = await _usersController.DeleteUser(userId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteUser_WhenGivenNonExistentUser_ShouldReturnNotFound()
        {
            // Arrange
            var nonExistentUserId = 999;

            // Act
            var result = await _usersController.DeleteUser(nonExistentUserId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion
    }
}