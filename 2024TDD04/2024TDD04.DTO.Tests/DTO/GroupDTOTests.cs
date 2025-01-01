using DTO;
using Xunit;

namespace _2024TDD04.DTO.Tests
{
    public class GroupDTOTests
    {
        [Fact]
        public void GroupDTO_WhenGivenValues_ShouldReturnInstantiatedGroupDTO()
        {
            // Arrange
            var expectedId = 1;
            var expectedName = "Test Group";
            var expectedAcronym = "TG";
            var expectedIsDeleted = false;

            // Act
            var groupDTO = new GroupDTO
            {
                Id = expectedId,
                Name = expectedName,
                Acronym = expectedAcronym,
                IsDeleted = expectedIsDeleted
            };

            // Assert
            Assert.Equal(expectedId, groupDTO.Id);
            Assert.Equal(expectedName, groupDTO.Name);
            Assert.Equal(expectedAcronym, groupDTO.Acronym);
            Assert.Equal(expectedIsDeleted, groupDTO.IsDeleted);
        }
    }
}
