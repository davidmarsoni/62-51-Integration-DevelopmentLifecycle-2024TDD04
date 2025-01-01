using DAL.Models;
using Xunit;

public class UserTests
{
    [Fact]
    public void User_WhenGivenValues_ShouldReturnInstantiatedUser()
    {
        // Arrange
        var expectedId = 1;
        var expectedUsername = "Test User";
        var expectedIsDeleted = false;
        
        // Act
        var user = new User
        {
            Id = expectedId,
            Username = expectedUsername,
            IsDeleted = expectedIsDeleted
        };

        // Assert
        Assert.Equal(expectedId, user.Id);
        Assert.Equal(expectedUsername, user.Username);
        Assert.Equal(expectedIsDeleted, user.IsDeleted);
        Assert.NotNull(user.Groups);
        Assert.Empty(user.Groups);
        Assert.NotNull(user.User_Groups);
        Assert.Empty(user.User_Groups);
    }
}
