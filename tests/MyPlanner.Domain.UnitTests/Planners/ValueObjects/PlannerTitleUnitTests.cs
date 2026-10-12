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
        public void Create_ShouldReturnFailureWithPlannerTitleRequiredError_WhenInputIsNullOrWhitespace(
            string? invalidTitle)
        {
            var result = PlannerTitle.Create(invalidTitle!);

            Assert.True(result.IsFailure);
            Assert.Equal(PlannerTitleErrors.TitleRequired, result.Error);
        }

        [Fact]
        public void Create_ShouldReturnFailureWithPlannerTitleTooLongError_WhenInputIsTooLong()
        {
            // Arrange
            var invalidTitle = new string('x', PlannerTitle.MaxLength + 1);

            // Act
            var result = PlannerTitle.Create(invalidTitle);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PlannerTitleErrors.TitleTooLong, result.Error);
        }

        [Fact]
        public void Create_ShouldReturnSuccessWithPlannerTitle_WhenInputIsValid()
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
        public void Create_ShouldReturnSuccessWithTrimmedPlannerTitle_WhenInputHasLeadingAndTrailingWhitespaces()
        {
            // Arrange
            var expectedValue = "Valid title";
            var inputWithWhitespace = $"   {expectedValue}   ";

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
