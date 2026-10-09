using MediatR;
using MyPlanner.Application.Behaviors;
using MyPlanner.Application.Errors;
using MyPlanner.Application.TestData.Behaviors;
using MyPlanner.Domain.Common;

namespace MyPlanner.Application.UnitTests.Behaviors
{
    public class ExceptionHandlingBehaviorUnitTests
    {
        private readonly ExceptionHandlingBehavior<TestExceptionHandlingCommand, Result> _sut = new();
        private readonly TestExceptionHandlingCommand _command = new();

        [Fact]
        public async Task Handle_ShouldReturnResult_WhenNoExceptionIsThrown()
        {
            // Arrange
            var nextCalled = false;
            RequestHandlerDelegate<Result> next = _ =>
            {
                nextCalled = true;
                return Task.FromResult(Result.Success());
            };

            // Act
            var result = await _sut.Handle(_command, next, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(nextCalled);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WhenAnExceptionIsThrown()
        {
            // Arrange
            var nextCalled = false;
            RequestHandlerDelegate<Result> next = _ =>
            {
                nextCalled = true;
                return Task.FromException<Result>(new Exception("Test exception"));
            };

            // Act
            var result = await _sut.Handle(_command, next, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(SystemErrors.UnexpectedError, result.Error);
            Assert.True(nextCalled);
        }
    }
}
