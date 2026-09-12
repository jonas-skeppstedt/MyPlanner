using MyPlanner.Domain.Common;

namespace MyPlanner.Domain.UnitTests.Common
{
    public class ErrorUnitTests
    {
        [Theory]
        [InlineData(ErrorType.NotFound)]
        [InlineData(ErrorType.Validation)]
        [InlineData(ErrorType.Conflict)]
        [InlineData(ErrorType.Unauthorized)]
        [InlineData(ErrorType.Forbidden)]
        [InlineData(ErrorType.Failure)]
        public void StaticFactories_ShouldCreateExpectedError(ErrorType errorType)
        {
            // Arrange
            var expectedCode = "Valid.Code";
            var expectedDescription = "Valid description";

            // Act
            var error = errorType switch
            {
                ErrorType.NotFound => Error.NotFound(expectedCode, expectedDescription),
                ErrorType.Validation => Error.Validation(expectedCode, expectedDescription),
                ErrorType.Conflict => Error.Conflict(expectedCode, expectedDescription),
                ErrorType.Unauthorized => Error.Unauthorized(expectedCode, expectedDescription),
                ErrorType.Forbidden => Error.Forbidden(expectedCode, expectedDescription),
                ErrorType.Failure => Error.Failure(expectedCode, expectedDescription),
                _ => throw new ArgumentOutOfRangeException(
                    nameof(errorType),
                    $"InlineData for '{errorType}' not mapped to any static factory method.")
            };

            // Assert
            Assert.Equal(errorType, error.Type);
            Assert.Equal(expectedCode, error.Code);
            Assert.Equal(expectedDescription, error.Description);
        }

        [Theory]
        [InlineData("Valid.Code", null)]
        [InlineData("Valid.Code", "")]
        [InlineData("Valid.Code", "   ")]
        [InlineData(null, "Valid description")]
        [InlineData("", "Valid description")]
        [InlineData("   ", "Valid description")]
        public void StaticFactories_ShouldThrowArgumentException_WhenCodeOrDescriptionIsInvalid(
            string? code,
            string? description)
        {
            Assert.ThrowsAny<ArgumentException>(() => Error.NotFound(code!, description!));
            Assert.ThrowsAny<ArgumentException>(() => Error.Validation(code!, description!));
            Assert.ThrowsAny<ArgumentException>(() => Error.Conflict(code!, description!));
            Assert.ThrowsAny<ArgumentException>(() => Error.Unauthorized(code!, description!));
            Assert.ThrowsAny<ArgumentException>(() => Error.Forbidden(code!, description!));
            Assert.ThrowsAny<ArgumentException>(() => Error.Failure(code!, description!));
        }

        [Fact]
        public void None_ShouldRepresentNoError()
        {
            // Arrange
            var expectedType = ErrorType.None;
            var expectedCode = string.Empty;
            var expectedDescription = string.Empty;

            // Act
            var error = Error.None;

            // Assert
            Assert.Equal(expectedType, error.Type);
            Assert.Equal(expectedCode, error.Code);
            Assert.Equal(expectedDescription, error.Description);
        }
    }
}
