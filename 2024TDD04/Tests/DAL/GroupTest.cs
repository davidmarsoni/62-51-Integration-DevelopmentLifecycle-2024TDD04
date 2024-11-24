using DAL.Models;
using Xunit;
using Group = DAL.Models.Group;

namespace Tests.DAL
{
    public class GroupTest
    {
        [Fact]
        public void Group_SetId_ShouldReturnCorrectValue()
        {
            // Arrange
            var group = new Group { Id = 1, Name = "Test Group", Acronym = "TG" };
            var expectedId = 1;

            // Act
            group.Id = expectedId;

            // Assert
            Assert.Equal(expectedId, group.Id);
        }

        [Fact]
        public void Group_SetName_ShouldReturnCorrectValue()
        {
            // Arrange
            var group = new Group { Id = 1, Name = "Test Group", Acronym = "TG" };
            var expectedName = "Test Group";

            // Act
            group.Name = expectedName;

            // Assert
            Assert.Equal(expectedName, group.Name);
        }

        [Fact]
        public void Group_SetAcronym_ShouldReturnCorrectValue()
        {
            // Arrange
            var group = new Group { Id = 1, Name = "Test Group", Acronym = "TG" };
            var expectedAcronym = "TG";

            // Act
            group.Acronym = expectedAcronym;

            // Assert
            Assert.Equal(expectedAcronym, group.Acronym);
        }

        [Fact]
        public void Group_SetIsDeleted_ShouldReturnCorrectValue()
        {
            // Arrange
            var group = new Group { Id = 1, Name = "Test Group", Acronym = "TG", IsDeleted = true };
            var expectedIsDeleted = true;

            // Act
            group.IsDeleted = expectedIsDeleted;

            // Assert
            Assert.Equal(expectedIsDeleted, group.IsDeleted);
        }

        [Fact]
        public void Group_Users_ShouldBeInitialized()
        {
            // Arrange
            var group = new Group { Id = 1, Name = "Test Group", Acronym = "TG" };

            // Act & Assert
            Assert.NotNull(group.Users);
            Assert.Empty(group.Users);
        }

        [Fact]
        public void Group_UserGroups_ShouldBeInitialized()
        {
            // Arrange
            var group = new Group { Id = 1, Name = "Test Group", Acronym = "TG" };

            // Act & Assert
            Assert.NotNull(group.User_Groups);
            Assert.Empty(group.User_Groups);
        }
    }
}
