using MyPlanner.Domain.Common;

namespace MyPlanner.Domain.UnitTests.Common
{
    public class ResultTUnitTests
    {
        [Fact]
        public void Success_ShouldThrowArgumentNullException_WhenValueIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Result<string>.Success(null!));
        }

        [Fact]
        public void Success_ShouldThrowArgumentException_WhenValueIsError()
        {
            var error = Error.Validation("Valid.Code", "Valid description");

            Assert.Throws<ArgumentException>(() => Result<Error>.Success(error));
        }

        [Fact]
        public void Success_ShouldCreateSuccessResult()
        {
            // Arrange
            var expectedValue = "Valid value";

            // Act
            var result = Result<string>.Success(expectedValue);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsNoOp);
            Assert.False(result.IsFailure);
            Assert.Equal(Error.None, result.Error);
            Assert.Equal(expectedValue, result.Value);
        }

        [Fact]
        public void NoOp_ShouldCreateNoOpResult()
        {
            var result = Result<string>.NoOp();

            Assert.True(result.IsNoOp);
            Assert.False(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Equal(Error.None, result.Error);
        }

        [Fact]
        public void Failure_ShouldThrowArgumentNullException_WhenErrorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Result<string>.Failure(null!));
        }

        [Fact]
        public void Failure_ShouldThrowArgumentException_WhenErrorIsErrorNone()
        {
            Assert.Throws<ArgumentException>(() => Result<string>.Failure(Error.None));
        }

        [Fact]
        public void Failure_ShouldCreateFailureResult()
        {
            // Arrange
            var expectedError = Error.Validation("Valid.Code", "Valid description");

            // Act
            var result = Result<string>.Failure(expectedError);

            // Assert
            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.False(result.IsNoOp);
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public void AccessingValue_ShouldThrowInvalidOperationException_WhenResultIsNoOp()
        {
            var result = Result<string>.NoOp();

            Assert.Throws<InvalidOperationException>(() => result.Value);
        }

        [Fact]
        public void AccessingValue_ShouldThrowInvalidOperationException_WhenResultIsFailure()
        {
            // Arrange
            var error = Error.Validation("Valid.Code", "Valid description");

            // Act
            var result = Result<string>.Failure(error);

            // Assert
            Assert.Throws<InvalidOperationException>(() => result.Value);
        }

        [Fact]
        public void ImplicitOperator_ShouldCreateFailureResult_FromError()
        {
            // Arrange
            var expectedError = Error.Validation("Valid.Code", "Valid description");

            // Act
            Result<string> result = expectedError;

            // Assert
            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.False(result.IsNoOp);
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public void ImplicitOperator_ShouldThrowArgumentException_WhenErrorIsErrorNone()
        {
            Assert.Throws<ArgumentException>(() => { Result<string> _ = Error.None; });
        }
    }
}
