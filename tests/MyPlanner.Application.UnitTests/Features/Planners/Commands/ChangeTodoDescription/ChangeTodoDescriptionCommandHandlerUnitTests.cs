using Moq;
using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Features.Planners.Commands.ChangeTodoDescription;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Planners.ValueObjects;
using MyPlanner.Domain.Shared;
using MyPlanner.Domain.TestData.Planners.Builders;

namespace MyPlanner.Application.UnitTests.Features.Planners.Commands.ChangeTodoDescription
{
    public class ChangeTodoDescriptionCommandHandlerUnitTests
    {
        private readonly ChangeTodoDescriptionCommandHandler _sut;
        private readonly Mock<IPlannerRepository> _plannerRepositoryMock = new();
        private readonly Mock<IUserContext> _userContextMock = new();

        public ChangeTodoDescriptionCommandHandlerUnitTests()
        {
            _sut = new(_plannerRepositoryMock.Object, _userContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WhenTodoDescriptionValidationFails()
        {
            // Arrange
            var tooLongDescription = new string('x', TodoDescription.MaxLength + 1);
            var invalidCommand = new ChangeTodoDescriptionCommand(
                PlannerId.New(),
                TodoId.New(),
                tooLongDescription);

            // Act
            var result = await _sut.Handle(invalidCommand, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(TodoDescriptionErrors.DescriptionTooLong, result.Error);

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
            var command = new ChangeTodoDescriptionCommand(
                PlannerId.New(),
                TodoId.New(),
                "New description");

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
        public async Task Handle_ShouldReturnFailureResult_WhenTodoDoesNotExist()
        {
            // Arrange
            var planner = new PlannerBuilder().Build();

            var command = new ChangeTodoDescriptionCommand(
                planner.Id,
                TodoId.New(),
                "New description");

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
            Assert.True(result.IsFailure);
            Assert.Equal(TodoErrors.NotFound, result.Error);
        }

        [Fact]
        public async Task Handle_ShouldReturnNoOpResult_WhenNewDescriptionIsIdentical()
        {
            // Arrange
            var identicalDescription = "Identical description";
            var initialDescription = TodoDescription.Create(identicalDescription).Value;

            var todo = new TodoBuilder()
                .WithTodoDescription(initialDescription)
                .Build();

            var planner = new PlannerBuilder()
                .WithTodos(todo)
                .Build();

            var command = new ChangeTodoDescriptionCommand(
                planner.Id,
                todo.Id,
                identicalDescription);

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
            Assert.True(result.IsNoOp);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WhenInputIsValidNewDescription()
        {
            // Arrange
            var initialDescription = TodoDescription.Create("Initial description").Value;

            var todo = new TodoBuilder()
                .WithTodoDescription(initialDescription)
                .Build();

            var planner = new PlannerBuilder()
                .WithTodos(todo)
                .Build();

            var command = new ChangeTodoDescriptionCommand(
                planner.Id,
                todo.Id,
                "New description");

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
            Assert.Equal(command.TodoId, result.Value.TodoId);
            Assert.Equal(command.NewDescription, result.Value.NewDescription);
        }
    }
}
