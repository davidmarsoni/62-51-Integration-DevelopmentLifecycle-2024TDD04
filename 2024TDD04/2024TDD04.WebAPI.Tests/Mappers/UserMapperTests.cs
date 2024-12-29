
using DAL.Models;
using DTO;
using WebApi.Mapper;

namespace _2024TDD04.WebAPI.Tests.Mappers
{
    public class UserMapperTests
    {
        #region ToDTO

        [Fact]
        public static void ToDTO_WhenGivenUser_ShouldReturnUserDTO()
        {
            // Arrange
            User user = new User
            {
                Id = 1,
                Username = "TestUser",
                IsDeleted = false
            };

            // Act
            var result = UserMapper.toDTO(user);

            // Assert
            Assert.IsType<UserDTO>(result);

            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.IsDeleted, result.IsDeleted);
        }   

        #endregion  

        #region ToDAL

        [Fact]
        public static void ToDAL_WhenGivenUserDTO_ShouldReturnUser()
        {
            // Arrange
            UserDTO userDTO = new UserDTO
            {
                Id = 1,
                Username = "TestUser",
                IsDeleted = false
            };

            // Act
            var result = UserMapper.toDAL(userDTO);

            // Assert
            Assert.IsType<User>(result);

            Assert.Equal(userDTO.Id, result.Id);
            Assert.Equal(userDTO.Username, result.Username);
            Assert.Equal(userDTO.IsDeleted, result.IsDeleted);
        }

        #endregion
    }
}