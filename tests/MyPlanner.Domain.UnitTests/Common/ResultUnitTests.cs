using MyPlanner.Domain.Common;

namespace MyPlanner.Domain.UnitTests.Common
{
    public class ResultUnitTests
    {
        [Fact]
        public void Success_ShouldReturnSuccessResult()
        {
            var result = Result.Success();

            Assert.True(result.IsSuccess);
            Assert.False(result.IsNoOp);
            Assert.False(result.IsFailure);
            Assert.Equal(Error.None, result.Error);
        }

        [Fact]
        public void NoOp_ShouldReturnNoOpResult()
        {
            var result = Result.NoOp();

            Assert.True(result.IsNoOp);
            Assert.False(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Equal(Error.None, result.Error);
        }

        [Fact]
        public void Failure_ShouldThrowArgumentNullException_WhenErrorIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => Result.Failure(null!));
        }

        [Fact]
        public void Failure_ShouldThrowArgumentException_WhenErrorIsErrorNone()
        {
            Assert.Throws<ArgumentException>(() => Result.Failure(Error.None));
        }

        [Fact]
        public void Failure_ShouldCreateFailureResult()
        {
            // Arrange
            var expectedError = Error.Validation("Valid.Code", "Valid description");

            // Act
            var result = Result.Failure(expectedError);

            // Assert
            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.False(result.IsNoOp);
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public void ImplicitOperator_ShouldCreateFailureResult_FromError()
        {
            // Arrange
            var expectedError = Error.Validation("Valid.Code", "Valid description");

            // Act
            Result result = expectedError;

            // Assert
            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.False(result.IsNoOp);
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public void ImplicitOperator_ShouldThrowArgumentException_WhenErrorIsErrorNone()
        {
            Assert.Throws<ArgumentException>(() => { Result _ = Error.None; });
        }
    }
}
