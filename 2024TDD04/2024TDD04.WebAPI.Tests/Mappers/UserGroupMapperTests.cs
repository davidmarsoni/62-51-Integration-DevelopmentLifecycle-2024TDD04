
using DAL.Models;
using DTO;
using WebApi.Mapper;
using Xunit;

namespace _2024TDD04.WebAPI.Tests.Mappers
{
    public class UserGroupMapperTests
    {
        #region ToDTO

        [Fact]
        public static void ToDTO_WhenGivenUserGroup_ShouldReturnUserGroupDTO()
        {
            // Arrange
            UserGroup userGroup = new UserGroup
            {
                Id = 1,
                GroupId = 2,
                UserId = 3
            };
            Group group = new Group
            {
                Id = userGroup.GroupId,
                Name = "TestGroup"
            };
            User user = new User
            {
                Id = userGroup.UserId,
                Username = "TestUser"
            };

            // Act
            var result = UserGroupMapper.toDTO(userGroup, user, group);

            // Assert
            Assert.IsType<UserGroupDTO>(result);
            Assert.Equal(userGroup.Id, result.Id);
            Assert.Equal(userGroup.GroupId, result.GroupId);
            Assert.Equal(group.Name, result.Groupname);
            Assert.Equal(userGroup.UserId, result.UserId);
            Assert.Equal(user.Username, result.Username);
        }

        #endregion

        #region ToDAL

        [Fact]
        public static void ToDAL_WhenGivenUserGroupDTO_ShouldReturnUserGroup()
        {
            // Arrange
            UserGroupDTO userGroupDTO = new UserGroupDTO
            {
                Id = 1,
                GroupId = 2,
                UserId = 3
            };

            // Act
            var result = UserGroupMapper.toDAL(userGroupDTO);

            // Assert
            Assert.IsType<UserGroup>(result);
            Assert.Equal(userGroupDTO.Id, result.Id);
            Assert.Equal(userGroupDTO.GroupId, result.GroupId);
            Assert.Equal(userGroupDTO.UserId, result.UserId);
        }

        #endregion
    }
}