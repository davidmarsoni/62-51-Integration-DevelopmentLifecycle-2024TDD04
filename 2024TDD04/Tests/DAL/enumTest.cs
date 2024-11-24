using Xunit;
using DAL.Models;

namespace Tests.DAL
{
    public class EnumTest
    {
        [Fact]
        public void AccessType_Forbidden_ShouldHaveCorrectValue()
        {
            // Arrange
            var expectedValue = 0;

            // Act
            var actualValue = (int)AccessType.Forbidden;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void AccessType_Allowed_ShouldHaveCorrectValue()
        {
            // Arrange
            var expectedValue = 1;

            // Act
            var actualValue = (int)AccessType.Allowed;

            // Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public void AccessType_ShouldContainForbiddenAndAllowed()
        {
            // Arrange & Act
            var values = Enum.GetValues(typeof(AccessType)).Cast<AccessType>().ToList();

            // Assert
            Assert.Contains(AccessType.Forbidden, values);
            Assert.Contains(AccessType.Allowed, values);
        }
    }
}
