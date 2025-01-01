
using DTO;
using Xunit;

namespace _2024TDD04.DTO.Tests
{
    public class UserDTOTests
    {
        [Fact]
        public void UserDTO_WhenGivenValues_ShouldReturnInstantiatedUserDTO()
        {
            // Arrange
            var expectedId = 1;
            var expectedUsername = "Test User";
            var expectedIsDeleted = false;

            // Act
            var userDTO = new UserDTO
            {
                Id = expectedId,
                Username = expectedUsername,
                IsDeleted = expectedIsDeleted
            };

            // Assert
            Assert.Equal(expectedId, userDTO.Id);
            Assert.Equal(expectedUsername, userDTO.Username);
            Assert.Equal(expectedIsDeleted, userDTO.IsDeleted);
        }
    }
}