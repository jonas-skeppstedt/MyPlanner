using MediatR;
using Moq;
using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Behaviors;
using MyPlanner.Application.TestData.Behaviors;
using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners.Errors;

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
                .Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldNotCallSaveChangesAsync_WhenResponseIsFailure()
        {
            // Arrange
            var expectedError = PlannerTitleErrors.TitleRequired;
            RequestHandlerDelegate<Result> next = _ =>
            {
                return Task.FromResult(Result.Failure(expectedError));
            };

            // Act
            var result = await _sut.Handle(_command, next, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);

            // Verify
            _unitOfWorkMock
                .Verify(x => x.SaveChangesAsync(), Times.Never);
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
                .Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}
