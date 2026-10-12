using Moq;
using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Features.Planners.Commands.ChangeTodoTitle;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Planners.ValueObjects;
using MyPlanner.Domain.Shared;
using MyPlanner.Domain.TestData.Planners.Builders;

namespace MyPlanner.Application.UnitTests.Features.Planners.Commands.ChangeTodoTitle
{
    public class ChangeTodoTitleCommandHandlerUnitTests
    {
        private readonly ChangeTodoTitleCommandHandler _sut;
        private readonly Mock<IPlannerRepository> _plannerRepositoryMock = new();
        private readonly Mock<IUserContext> _userContextMock = new();

        public ChangeTodoTitleCommandHandlerUnitTests()
        {
            _sut = new(_plannerRepositoryMock.Object, _userContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenTodoTitleValidationFails()
        {
            // Arrange
            var command = new ChangeTodoTitleCommand(PlannerId.New(), TodoId.New(), NewTitle: null!);

            // Act
            var result = await _sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(TodoTitleErrors.TitleRequired, result.Error);

            // Verify
            _plannerRepositoryMock
                .Verify(x => x.GetByIdAsync(
                    It.IsAny<PlannerId>(),
                    It.IsAny<UserId>(),
                    It.IsAny<CancellationToken>()),
                    Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureWithPlannerNotFoundError_WhenPlannerWasNotFound()
        {
            // Arrange
            var command = new ChangeTodoTitleCommand(PlannerId.New(), TodoId.New(), "New title");

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
        public async Task Handle_ShouldReturnFailureWithTodoNotFoundError_WhenTodoDoesNotExist()
        {
            // Arrange
            var planner = new PlannerBuilder().Build();

            var command = new ChangeTodoTitleCommand(planner.Id, TodoId.New(), "New title");

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
        public async Task Handle_ShouldReturnNoOp_WhenNewTitleIsIdentical()
        {
            // Arrange
            var identicalTitle = "Identical title";
            var initialTitle = TodoTitle.Create(identicalTitle).Value;

            var todo = new TodoBuilder()
                .WithTodoTitle(initialTitle)
                .Build();

            var planner = new PlannerBuilder()
                .WithTodos(todo)
                .Build();

            var command = new ChangeTodoTitleCommand(planner.Id, todo.Id, identicalTitle);

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
        public async Task Handle_ShouldReturnSuccessWithResponse_WhenTitleIsChanged()
        {
            // Arrange
            var initialTitle = TodoTitle.Create("Initial title").Value;

            var todo = new TodoBuilder()
                .WithTodoTitle(initialTitle)
                .Build();

            var planner = new PlannerBuilder()
                .WithTodos(todo)
                .Build();

            var command = new ChangeTodoTitleCommand(planner.Id, todo.Id, "New title");

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
            Assert.Equal(command.NewTitle, result.Value.NewTitle);
        }
    }
}
