using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Planners.ValueObjects;

namespace MyPlanner.Domain.UnitTests.Planners.ValueObjects
{
    public class TodoDescriptionUnitTests
    {
        [Fact]
        public void Create_ShouldReturnExpectedFailureResult_WhenInputIsTooLong()
        {
            // Arrange
            var expectedError = TodoDescriptionErrors.DescriptionTooLong;
            var invalidDescription = new string('x', TodoDescription.MaxLength + 1);

            // Act
            var result = TodoDescription.Create(invalidDescription);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public void Create_ShouldReturnSuccessResult_WhenInputIsValid()
        {
            // Arrange
            var expectedValue = "Valid description";

            // Act
            var result = TodoDescription.Create(expectedValue);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedValue, result.Value.Value);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_ShouldReturnTodoDescriptionEmpty_WhenInputStringIsNullOrEmpty(string? description)
        {
            // Arrange
            var expectedValue = TodoDescription.Empty;

            // Act
            var result = TodoDescription.Create(description!);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedValue, result.Value);
        }


        [Fact]
        public void Create_ShouldReturnSuccessResultWithTrimmedValue_WhenInputHasLeadingAndTrailingWhitespaces()
        {
            // Arrange
            var whitespace = new string(' ', TodoDescription.MaxLength);
            var expectedValue = "Valid description";
            var inputWithWhitespace = whitespace + expectedValue + whitespace;

            // Act
            var result = TodoDescription.Create(inputWithWhitespace);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedValue, result.Value.Value);
        }

        [Fact]
        public void ToString_ShouldReturnRawValue()
        {
            // Arrange
            var expectedValue = "Valid description";
            var todoDescription = TodoDescription.Create(expectedValue).Value;

            // Act
            var result = todoDescription.ToString();

            // Assert
            Assert.Equal(expectedValue, result);
        }
    }
}
