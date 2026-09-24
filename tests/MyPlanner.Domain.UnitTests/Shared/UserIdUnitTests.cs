using MyPlanner.Domain.Shared;

namespace MyPlanner.Domain.UnitTests.Shared
{
    public class UserIdUnitTests
    {
        [Fact]
        public void TryParse_ShouldParseStringToUserId_WhenStringIsGuidFormat()
        {
            // Arrange
            var expectedGuid = Guid.NewGuid();
            var inputString = expectedGuid.ToString();

            // Act
            var result = UserId.TryParse(inputString, out var userId);

            // Assert
            Assert.True(result);
            Assert.Equal(expectedGuid, userId.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("not-a-guid")]
        public void TryParse_ShouldReturnFalseAndDefault_WhenStringIsInvalid(string? invalidString)
        {
            // Act
            var result = UserId.TryParse(invalidString!, out var userId);

            // Assert
            Assert.False(result);
            Assert.Equal(default, userId.Value);
        }

        [Fact]
        public void ThrowIfEmpty_ShouldThrowArgumentException_WhenPlannerIdIsEmpty()
        {
            // Arrange
            UserId emptyUserId = default;
            var paramName = "userId";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => emptyUserId.ThrowIfEmpty(paramName));
        }

        [Fact]
        public void ToString_ShouldReturnRawValue()
        {
            // Arrange
            var expectedGuid = Guid.NewGuid();
            var userId = new UserId(expectedGuid);

            // Act
            var result = userId.ToString();

            // Assert
            Assert.Equal(expectedGuid.ToString(), result);
        }
    }
}
