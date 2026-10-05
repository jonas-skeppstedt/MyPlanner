using Moq;
using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Features.Planners.Commands.ChangePlannerTitle;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Planners.ValueObjects;
using MyPlanner.Domain.Shared;
using MyPlanner.Domain.TestData.Planners.Builders;

namespace MyPlanner.Application.UnitTests.Features.Planners.Commands.ChangePlannerTitle
{
    public class ChangePlannerTitleCommandHandlerUnitTests
    {
        private readonly ChangePlannerTitleCommandHandler _sut;
        private readonly Mock<IPlannerRepository> _plannerRepositoryMock = new();
        private readonly Mock<IUserContext> _userContextMock = new();

        public ChangePlannerTitleCommandHandlerUnitTests()
        {
            _sut = new(_plannerRepositoryMock.Object, _userContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WhenPlannerTitleValidationFails()
        {
            // Arrange
            var invalidCommand = new ChangePlannerTitleCommand(PlannerId.New(), NewTitle: null!);

            // Act
            var result = await _sut.Handle(invalidCommand, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PlannerTitleErrors.TitleRequired, result.Error);

            // Verify
            _plannerRepositoryMock
                .Verify(x => x.GetByIdAsync(
                    It.IsAny<PlannerId>(),
                    It.IsAny<UserId>(),
                    It.IsAny<CancellationToken>()),
                    Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WhenPlannerWasNotFound()
        {
            // Arrange
            var command = new ChangePlannerTitleCommand(PlannerId.New(), "New title");

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
        }

        [Fact]
        public async Task Handle_ShouldReturnNoOpResult_WhenTitleIsIdentical()
        {
            // Arrange
            var identicalTitle = "Identical title";
            var initialTitle = PlannerTitle.Create(identicalTitle).Value;

            var planner = new PlannerBuilder()
                .WithPlannerTitle(initialTitle)
                .Build();

            var command = new ChangePlannerTitleCommand(planner.Id, identicalTitle);

            _userContextMock
                .Setup(x => x.UserId)
                .Returns(planner.OwnerId);

            _plannerRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    planner.Id,
                    planner.OwnerId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(planner);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsNoOp);
            Assert.Equal(initialTitle, planner.Title);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WhenInputIsValidNewTitle()
        {
            // Arrange
            var initialTitle = PlannerTitle.Create("Initial title").Value;

            var planner = new PlannerBuilder()
                .WithPlannerTitle(initialTitle)
                .Build();

            var command = new ChangePlannerTitleCommand(planner.Id, "New title");

            _userContextMock
                .Setup(x => x.UserId)
                .Returns(planner.OwnerId);

            _plannerRepositoryMock
                .Setup(x => x.GetByIdAsync(
                    planner.Id,
                    planner.OwnerId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(planner);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(command.NewTitle, result.Value.NewTitle);

            Assert.Equal(command.NewTitle, planner.Title.Value);
        }
    }
}
