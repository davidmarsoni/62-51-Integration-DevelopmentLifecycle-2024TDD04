
using DAL.Models;
using DTO;
using WebAPI.Mapper;
using Xunit;

namespace _2024TDD04.WebAPI.Tests.Mappers
{
    public class AccessMapperTests
    {
        #region ToDTO

        [Fact]
        public void ToDTO_WhenGivenAccess_ShouldReturnAccessDTO()
        {
            // Arrange
            Access access = new Access
            {
                Id = 1,
                RoomId = 2,
                GroupId = 3
            };

            // Act
            var result = AccessMapper.toDTO(access);

            // Assert
            Assert.IsType<AccessDTO>(result);
            Assert.Equal(access.Id, result.Id);
            Assert.Equal(access.RoomId, result.RoomId);
            Assert.Equal(access.GroupId, result.GroupId);
        }

        #endregion

        #region ToDAL

        [Fact]
        public void ToDAL_WhenGivenAccessDTO_ShouldReturnAccess()
        {
            // Arrange
            AccessDTO accessDTO = new AccessDTO
            {
                Id = 1,
                RoomId = 2,
                GroupId = 3
            };

            // Act
            var result = AccessMapper.toDAL(accessDTO);

            // Assert
            Assert.IsType<Access>(result);
            Assert.Equal(accessDTO.Id, result.Id);
            Assert.Equal(accessDTO.RoomId, result.RoomId);
            Assert.Equal(accessDTO.GroupId, result.GroupId);
        }

        #endregion
    }
}