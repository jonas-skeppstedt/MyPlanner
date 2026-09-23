using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Planners.ValueObjects;

namespace MyPlanner.Domain.UnitTests.Planners.ValueObjects
{
    public class PlannerTitleUnitTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_ShouldReturnExpectedFailureResult_WhenInputIsNullOrWhitespace(string? title)
        {
            // Arrange
            var expectedError = PlannerTitleErrors.TitleRequired;

            // Act
            var result = PlannerTitle.Create(title!);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public void Create_ShouldReturnExpectedFailureResult_WhenInputIsTooLong()
        {
            // Arrange
            var expectedError = PlannerTitleErrors.TitleTooLong;
            var invalidTitle = new string('x', PlannerTitle.MaxLength + 1);

            // Act
            var result = PlannerTitle.Create(invalidTitle);

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
            var result = PlannerTitle.Create(expectedValue);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedValue, result.Value.Value);
        }

        [Fact]
        public void Create_ShouldReturnSuccessResultWithTrimmedValue_WhenInputHasLeadingAndTrailingWhitespaces()
        {
            // Arrange
            var whitespace = new string(' ', PlannerTitle.MaxLength);
            var expectedValue = "Valid title";
            var inputWithWhitespace = whitespace + expectedValue + whitespace;

            // Act
            var result = PlannerTitle.Create(inputWithWhitespace);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedValue, result.Value.Value);
        }

        [Fact]
        public void ToString_ShouldReturnRawValue()
        {
            // Arrange
            var expectedValue = "Valid title";
            var plannerTitle = PlannerTitle.Create(expectedValue).Value;

            // Act
            var result = plannerTitle.ToString();

            // Assert
            Assert.Equal(expectedValue, result);
        }
    }
}
