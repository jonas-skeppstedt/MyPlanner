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
        public void Create_ShouldReturnFailureWithTodoTitleRequiredError_WhenInputIsNullOrWhitespace(
            string? invalidTitle)
        {
            var result = TodoTitle.Create(invalidTitle!);

            Assert.True(result.IsFailure);
            Assert.Equal(TodoTitleErrors.TitleRequired, result.Error);
        }

        [Fact]
        public void Create_ShouldReturnFailureWithTodoTitleTooLongError_WhenInputIsTooLong()
        {
            // Arrange
            var invalidTitle = new string('x', TodoTitle.MaxLength + 1);

            // Act
            var result = TodoTitle.Create(invalidTitle);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(TodoTitleErrors.TitleTooLong, result.Error);
        }

        [Fact]
        public void Create_ShouldReturnSuccessWithTodoTitle_WhenInputIsValid()
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
        public void Create_ShouldReturnSuccessWithTrimmedTodoTitle_WhenInputHasLeadingAndTrailingWhitespaces()
        {
            // Arrange
            var expectedValue = "Valid title";
            var inputWithWhitespace = $"   {expectedValue}   ";

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
