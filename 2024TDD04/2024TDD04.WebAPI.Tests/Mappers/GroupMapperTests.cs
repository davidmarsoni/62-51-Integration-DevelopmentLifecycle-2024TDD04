using DAL.Models;
using DTO;
using WebApi.Mapper;
using Xunit;

namespace _2024TDD04.WebAPI.Tests.Mappers
{
    public class GroupMapperTests
    {
        #region ToDTO

        [Fact]
        public static void ToDTO_WhenGivenGroup_ShouldReturnGroupDTO()
        {
            // Arrange
            Group group = new Group
            {
                Id = 1,
                Name = "TestGroup",
                Acronym = "TG",
                IsDeleted = false
            };

            // Act
            var result = GroupMapper.toDTO(group);

            // Assert
            Assert.IsType<GroupDTO>(result);
            Assert.Equal(group.Id, result.Id);
            Assert.Equal(group.Name, result.Name);
            Assert.Equal(group.Acronym, result.Acronym);
            Assert.Equal(group.IsDeleted, result.IsDeleted);
        }

        #endregion

        #region ToDAL

        [Fact]
        public static void ToDAL_WhenGivenGroupDTO_ShouldReturnGroup()
        {
            // Arrange
            GroupDTO groupDTO = new GroupDTO
            {
                Id = 1,
                Name = "TestGroupDTO",
                Acronym = "TGD",
                IsDeleted = false
            };

            // Act
            var result = GroupMapper.toDAL(groupDTO);

            // Assert
            Assert.IsType<Group>(result);
            Assert.Equal(groupDTO.Id, result.Id);
            Assert.Equal(groupDTO.Name, result.Name);
            Assert.Equal(groupDTO.Acronym, result.Acronym);
            Assert.Equal(groupDTO.IsDeleted, result.IsDeleted);
        }

        #endregion
    }
}