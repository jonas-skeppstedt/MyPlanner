using Moq;
using MyPlanner.Application.Abstractions;
using MyPlanner.Application.Features.Planners.Commands.AddTodo;
using MyPlanner.Application.TestData.Planners.TheoryData;
using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Planners.ValueObjects;
using MyPlanner.Domain.Shared;
using MyPlanner.Domain.TestData.Planners.Builders;

namespace MyPlanner.Application.UnitTests.Features.Planners.Commands.AddTodo
{
    public class AddTodoCommandHandlerUnitTests
    {
        private readonly AddTodoCommandHandler _sut;
        private readonly Mock<IPlannerRepository> _plannerRepositoryMock = new();
        private readonly Mock<IUserContext> _userContextMock = new();

        public AddTodoCommandHandlerUnitTests()
        {
            _sut = new(_plannerRepositoryMock.Object, _userContextMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WhenPlannerWasNotFound()
        {
            // Arrange
            var command = new AddTodoCommand(PlannerId.New(), "Valid title");

            _userContextMock
                .Setup(x => x.UserId)
                .Returns(new UserId(Guid.NewGuid()));

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

        [Theory]
        [ClassData(typeof(InvalidAddTodoCommandArgumentsData))]
        public async Task Handle_ShouldReturnFailureResult_WhenAnyDomainValidationFails(
            string title,
            Error expectedError)
        {
            // Arrange
            var planner = new PlannerBuilder().Build();
            var command = new AddTodoCommand(planner.Id, title);

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
            Assert.Equal(expectedError, result.Error);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WhenTodoIsAddedToPlanner()
        {
            // Arrange
            var planner = new PlannerBuilder().Build();
            var command = new AddTodoCommand(planner.Id, "Valid title");

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
            Assert.NotEqual(default, result.Value.TodoId);
            Assert.Equal(command.Title, result.Value.Title);

            var todo = Assert.Single(planner.Todos);
            Assert.Equal(todo.Id, result.Value.TodoId);
            Assert.Equal(TodoDescription.Empty.Value, result.Value.Description);
        }
    }
}
