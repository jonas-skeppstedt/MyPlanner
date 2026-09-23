using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Planners.ValueObjects;

namespace MyPlanner.Domain.UnitTests.Planners.ValueObjects
{
    public class TodoTitleUnitTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_ShouldReturnExpectedFailureResult_WhenInputIsNullOrWhitespace(string? title)
        {
            // Arrange
            var expectedError = TodoTitleErrors.TitleRequired;

            // Act
            var result = TodoTitle.Create(title!);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public void Create_ShouldReturnExpectedFailureResult_WhenInputIsTooLong()
        {
            // Arrange
            var expectedError = TodoTitleErrors.TitleTooLong;
            var invalidTitle = new string('x', TodoTitle.MaxLength + 1);

            // Act
            var result = TodoTitle.Create(invalidTitle);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public void Create_ShouldReturnSuccessResult_WhenInputIsValid()
        {
            // Arrange
            var expectedValue = "Valid title";

            // Act
            var result = TodoTitle.Create(expectedValue);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedValue, result.Value.Value);
        }

        [Fact]
        public void Create_ShouldReturnSuccessResultWithTrimmedValue_WhenInputHasLeadingAndTrailingWhitespaces()
        {
            // Arrange
            var whitespace = new string(' ', TodoTitle.MaxLength);
            var expectedValue = "Valid title";
            var inputWithWhitespace = whitespace + expectedValue + whitespace;

            // Act
            var result = TodoTitle.Create(inputWithWhitespace);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedValue, result.Value.Value);
        }

        [Fact]
        public void ToString_ShouldReturnRawValue()
        {
            // Arrange
            var expectedValue = "Valid title";
            var todoTitle = TodoTitle.Create(expectedValue).Value;

            // Act
            var result = todoTitle.ToString();

            // Assert
            Assert.Equal(expectedValue, result);
        }
    }
}
