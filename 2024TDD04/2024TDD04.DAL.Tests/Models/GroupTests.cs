using DAL.Models;
using Xunit;
using Group = DAL.Models.Group;

namespace _2024TDD04.DAL.Tests.Models
{
    public class GroupTests
    {
        [Fact]
        public void Group_WhenGivenValues_ShouldReturnInstantiatedGroup()
        {
            // Arrange
            var expectedId = 1;
            var expectedName = "Test Group";
            var expectedAcronym = "TG";
            var expectedIsDeleted = false;

            // Act
            var group = new Group
            {
                Id = expectedId,
                Name = expectedName,
                Acronym = expectedAcronym,
                IsDeleted = expectedIsDeleted
            };

            // Assert
            Assert.Equal(expectedId, group.Id);
            Assert.Equal(expectedName, group.Name);
            Assert.Equal(expectedAcronym, group.Acronym);
            Assert.Equal(expectedIsDeleted, group.IsDeleted);
            Assert.NotNull(group.Users);
            Assert.Empty(group.Users);
            Assert.NotNull(group.User_Groups);
            Assert.Empty(group.User_Groups);
        }
    }
}
