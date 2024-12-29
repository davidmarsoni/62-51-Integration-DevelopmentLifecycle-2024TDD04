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

namespace _2024TDD04.WebAPI.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly UsersController _usersController;
        private readonly RoomAccessContext _testDbContext;

        public UsersControllerTests()
        {
            // ...existing code for in-memory DB...
            _testDbContext = InMemoryRoomContext.CreateInMemoryContext();
            _usersController = new UsersController(_testDbContext);
        }

        #region GetUsers

        [Fact]
        public async void GetUsers_WhenHasUser_ShouldReturnListOfUsers()
        {
            // Arrange

            // Act
            var result = await _usersController.GetUsers();

            // Assert
            Assert.IsType<ActionResult<IEnumerable<UserDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(4, result.Value.Count());
        }

        public async void GetUsers_WhenHasNoUsers_ShouldReturnEmptyList()
        {
            // Arrange
            _testDbContext.Users.RemoveRange(_testDbContext.Users);
            _testDbContext.SaveChanges();

            // Act
            var result = await _usersController.GetUsers();

            // Assert
            Assert.IsType<ActionResult<List<UserDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
        }



        #endregion

        #region GetUsernameExist

        [Fact]
        public async void GetUsernameExist_UsernameExists_ShouldReturnTrue()
        {
            // Arrange

            // Act
            var result = await _usersController.UsernameExist("Widmer");

            // Assert
            Assert.IsType<ActionResult<bool>>(result);
            Assert.True(result.Value);
        }

        [Fact]
        public async void GetUsernameExist_UsernameDoesNotExist_ShouldReturnFalse()
        {
            // Arrange

            // Act
            var result = await _usersController.UsernameExist("TestUser");

            // Assert
            Assert.IsType<ActionResult<bool>>(result);
            Assert.False(result.Value);
        }

        #endregion

        #region GetUsersActive

        [Fact]
        public async void GetUsersActive_ShouldReturnListOfUsers()
        {
            // Arrange

            // Act
            var result = await _usersController.GetUsersActive();

            // Assert
            Assert.IsType<ActionResult<List<UserDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(3, result.Value.Count);
        }

        [Fact]
        public async void GetUsersActive_NoActiveUsers_ShouldReturnEmptyList()
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
            Assert.IsType<ActionResult<List<UserDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async void GetUsersActive_NoUsers_ShouldReturnEmptyList()
        {
            // Arrange
            _testDbContext.Users.RemoveRange(_testDbContext.Users);
            _testDbContext.SaveChanges();

            // Act
            var result = await _usersController.GetUsersActive();

            // Assert
            Assert.IsType<ActionResult<List<UserDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
        }

        #endregion

        #region GetUsersByGroupId

        [Fact]
        public async void GetUsersByGroupId_GroupExists_ShouldReturnListOfUsers()
        {
            // Arrange

            // Act
            var result = await _usersController.GetUsersByGroupId(1);

            // Assert
            Assert.IsType<ActionResult<IEnumerable<UserDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.NotEmpty(result.Value);
        }

        [Fact]
        public async void GetUsersByGroupId_GroupDoesNotExist_ShouldReturnEmptyList()
        {
            // Arrange

            // Act
            var result = await _usersController.GetUsersByGroupId(999);

            // Assert
            Assert.IsType<ActionResult<IEnumerable<UserDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async void GetUsersByGroupId_GroupExistsButNoUsers_ShouldReturnEmptyList()
        {
            // Arrange

            // Act
            var result = await _usersController.GetUsersByGroupId(4);

            // Assert
            Assert.IsType<ActionResult<IEnumerable<UserDTO>>>(result);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value);
        }

        #endregion

        #region GetUser

        [Fact]
        public async void GetUser_UserExists_ShouldReturnUserDTO()
        {
            // Arrange

            // Act
            var result = await _usersController.GetUser(1);

            // Assert
            Assert.IsType<ActionResult<UserDTO>>(result);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Id);
        }

        [Fact]
        public async void GetUser_UserDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange

            // Act
            var result = await _usersController.GetUser(999);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        #endregion

        #region PutUser

        [Fact]
        public async void PutUser_UserExists_ShouldReturnNoContentAndEntryIsUpdated()
        {
            // Arrange
            UserDTO userDTO = new UserDTO
            {
                Id = 1,
                Username = "TestUser",
                IsDeleted = false
            };

            // Act
            var result = await _usersController.PutUser(1, userDTO);

            // Assert
            Assert.IsType<NoContentResult>(result);

            User user = await _testDbContext.Users.FindAsync(1);
            Assert.NotNull(user);
            Assert.Equal(userDTO.Username, user.Username);
        }

        [Fact]
        public async void PutUser_UserDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            UserDTO userDTO = new UserDTO
            {
                Id = 999,
                Username = "TestUser",
                IsDeleted = false
            };

            // Act
            var result = await _usersController.PutUser(999, userDTO);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void PutUser_UserIdProvidedIsDifferentFromUserDTOId_ShouldReturnBadRequest()
        {
            // Arrange
            UserDTO userDTO = new UserDTO
            {
                Id = 1,
                Username = "TestUser",
                IsDeleted = false
            };

            // Act
            var result = await _usersController.PutUser(999, userDTO);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        #endregion

        #region PostUser

        [Fact]
        public async void PostUser_UserDoesNotExist_ShouldReturnCreatedAtActionResult()
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
        public async void PostUser_UserExists_ShouldReturnConflict()
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
            Assert.IsType<ConflictResult>(result.Result);
        }

        #endregion

        #region DeleteUser

        [Fact]
        public async void DeleteUser_UserExists_ShouldReturnAnEmptyStatusCode()
        {
            // Arrange

            // Act
            var result = await _usersController.DeleteUser(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async void DeleteUser_UserDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange

            // Act
            var result = await _usersController.DeleteUser(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        #endregion
    }
}