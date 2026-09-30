using MediatR;
using Moq;
using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Behaviors;
using MyPlanner.Application.Errors;
using MyPlanner.Application.UnitTests.TestData;
using MyPlanner.Domain.Common;
using MyPlanner.Domain.Shared;

namespace MyPlanner.Application.UnitTests.Behaviors
{
    public class AuthenticationBehaviorUnitTests
    {
        private readonly AuthenticationBehavior<TestAuthenticationCommand, Result> _sut;
        private readonly Mock<IUserContext> _userContextMock = new();
        private readonly TestAuthenticationCommand _command = new();

        public AuthenticationBehaviorUnitTests()
        {
            _sut = new(_userContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnExpectedError_WhenCurrentUserIsNotAuthenticated()
        {
            // Arrange
            UserId defaultUserId = default;
            var nextCalled = false;
            RequestHandlerDelegate<Result> next = _ =>
            {
                nextCalled = true;
                return Task.FromResult(Result.Success());
            };

            _userContextMock
                .Setup(x => x.UserId)
                .Returns(defaultUserId);

            // Act
            var result = await _sut.Handle(_command, next, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(AuthenticationErrors.UserNotAuthenticated, result.Error);
            Assert.False(nextCalled);
        }

        [Fact]
        public async Task Handle_ShouldCallNext_WhenCurrentUserIsAuthenticated()
        {
            // Arrange
            var validUserId = new UserId(Guid.NewGuid());
            var nextCalled = false;
            RequestHandlerDelegate<Result> next = _ =>
            {
                nextCalled = true;
                return Task.FromResult(Result.Success());
            };

            _userContextMock
                .Setup(x => x.UserId)
                .Returns(validUserId);

            // Act
            var result = await _sut.Handle(_command, next, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(nextCalled);
        }
    }
}
