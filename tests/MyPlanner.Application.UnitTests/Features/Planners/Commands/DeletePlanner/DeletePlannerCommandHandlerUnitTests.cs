using Moq;
using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Features.Planners.Commands.DeletePlanner;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Shared;
using MyPlanner.Domain.TestData.Planners.Builders;

namespace MyPlanner.Application.UnitTests.Features.Planners.Commands.DeletePlanner
{
    public class DeletePlannerCommandHandlerUnitTests
    {
        private readonly DeletePlannerCommandHandler _sut;
        private readonly Mock<IPlannerRepository> _plannerRepositoryMock = new();
        private readonly Mock<IUserContext> _userContextMock = new();

        public DeletePlannerCommandHandlerUnitTests()
        {
            _sut = new(_plannerRepositoryMock.Object, _userContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WhenPlannerWasNotFound()
        {
            // Arrange
            var planner = new PlannerBuilder().Build();

            var command = new DeletePlannerCommand(PlannerId.New());

            _plannerRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    It.IsAny<PlannerId>(),
                    It.IsAny<UserId>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Planner?)null);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PlannerErrors.NotFound, result.Error);

            //Verify
            _plannerRepositoryMock.Verify(
                x => x.Remove(It.IsAny<Planner>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WhenUserIsNotOwner()
        {
            //Arrange
            var planner = new PlannerBuilder().Build();
            var notOwnerId = new UserId(Guid.NewGuid());

            var command = new DeletePlannerCommand(planner.Id);

            _userContextMock
                .Setup(x => x.UserId)
                .Returns(notOwnerId);

            _plannerRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    command.PlannerId,
                    It.IsAny<UserId>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(planner);

            //Act
            var result = await _sut.Handle(command, CancellationToken.None);

            //Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PlannerErrors.OnlyOwnerCanDelete, result.Error);

            //Verify
            _plannerRepositoryMock.Verify(
                x => x.Remove(It.IsAny<Planner>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WhenDeleteIsSuccessful()
        {
            // Arrange
            var planner = new PlannerBuilder().Build();

            var command = new DeletePlannerCommand(planner.Id);

            _userContextMock
                .Setup(x => x.UserId)
                .Returns(planner.OwnerId);

            _plannerRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    command.PlannerId,
                    planner.OwnerId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(planner);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            //Verify
            _plannerRepositoryMock.Verify(
                x => x.Remove(planner), Times.Once);
        }
    }
}
