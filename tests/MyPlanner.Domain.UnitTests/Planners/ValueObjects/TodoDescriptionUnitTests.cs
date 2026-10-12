using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Planners.ValueObjects;

namespace MyPlanner.Domain.UnitTests.Planners.ValueObjects
{
    public class TodoDescriptionUnitTests
    {
        [Fact]
        public void Create_ShouldReturnFailureWithTodoDescriptionTooLongError_WhenInputIsTooLong()
        {
            // Arrange
            var invalidDescription = new string('x', TodoDescription.MaxLength + 1);

            // Act
            var result = TodoDescription.Create(invalidDescription);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(TodoDescriptionErrors.DescriptionTooLong, result.Error);
        }

        [Fact]
        public void Create_ShouldReturnSuccessWithTodoDescription_WhenInputIsValid()
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
        public void Create_ShouldReturnSuccessWithTodoDescriptionEmpty_WhenInputStringIsNullOrEmpty(
            string? emptyDescription)
        {
            var result = TodoDescription.Create(emptyDescription!);

            Assert.True(result.IsSuccess);
            Assert.Equal(TodoDescription.Empty, result.Value);
        }

        [Fact]
        public void Create_ShouldReturnSuccessWithTrimmedTodoDescription_WhenInputHasLeadingAndTrailingWhitespaces()
        {
            // Arrange
            var expectedValue = "Valid description";
            var inputWithWhitespace = $"   {expectedValue}   ";

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
