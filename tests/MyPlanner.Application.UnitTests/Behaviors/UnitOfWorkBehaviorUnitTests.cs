using MediatR;
using Moq;
using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Behaviors;
using MyPlanner.Application.UnitTests.TestData;
using MyPlanner.Domain.Common;

namespace MyPlanner.Application.UnitTests.Behaviors
{
    public class UnitOfWorkBehaviorUnitTests
    {
        private readonly UnitOfWorkBehavior<TestUnitOfWorkCommand, Result> _sut;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly TestUnitOfWorkCommand _command = new();

        public UnitOfWorkBehaviorUnitTests()
        {
            _sut = new(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallSaveChangesAsync_WhenResponseIsSuccess()
        {
            // Arrange
            RequestHandlerDelegate<Result> next = _ =>
            {
                return Task.FromResult(Result.Success());
            };

            // Act
            var result = await _sut.Handle(_command, next, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            // Verify
            _unitOfWorkMock
                .Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldNotCallSaveChangesAsync_WhenResponseIsFailure()
        {
            // Arrange
            var expectedError = Error.Validation("Test.Error", "A test error.");
            RequestHandlerDelegate<Result> next = _ =>
            {
                return Task.FromResult(Result.Failure(expectedError));
            };

            // Act
            var result = await _sut.Handle(_command, next, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(expectedError, result.Error);

            // Verify
            _unitOfWorkMock
                .Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldNotCallSaveChangesAsync_WhenResponseIsNoOp()
        {
            // Arrange
            RequestHandlerDelegate<Result> next = _ =>
            {
                return Task.FromResult(Result.NoOp());
            };

            // Act
            var result = await _sut.Handle(_command, next, CancellationToken.None);

            // Assert
            Assert.True(result.IsNoOp);

            // Verify
            _unitOfWorkMock
                .Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
