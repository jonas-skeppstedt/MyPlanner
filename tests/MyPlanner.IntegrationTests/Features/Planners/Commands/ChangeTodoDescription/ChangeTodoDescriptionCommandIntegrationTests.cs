using MyPlanner.Application.Errors;
using MyPlanner.Application.Features.Planners.Commands.ChangeTodoDescription;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Planners.ValueObjects;
using MyPlanner.Domain.Shared;
using MyPlanner.Domain.TestData.Planners.Builders;
using MyPlanner.IntegrationTests.Setup;

namespace MyPlanner.IntegrationTests.Features.Planners.Commands.ChangeTodoDescription
{
    public class ChangeTodoDescriptionCommandIntegrationTests : IntegrationTestBase
    {
        public ChangeTodoDescriptionCommandIntegrationTests(IntegrationTestFixture integrationTestFixture)
            : base(integrationTestFixture)
        {
        }

        [Fact]
        public async Task Command_ShouldReturnUserNotAuthenticatedError_WhenUserIsNotAuthenticated()
        {
            // Arrange
            SetUnauthenticatedCurrentUser();

            var command = new ChangeTodoDescriptionCommand(PlannerId.New(), TodoId.New(), "Valid description");

            // Act
            var result = await Sender.Command(command);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(AuthenticationErrors.UserNotAuthenticated, result.Error);
        }

        [Fact]
        public async Task Command_ShouldReturnPlannerNotFoundError_WhenPlannerExistsButCurrentUserIsNotOwner()
        {
            // Arrange
            var initialDescription = TodoDescription.Create("Initial description").Value;
            var todo = new TodoBuilder()
                .WithTodoDescription(initialDescription)
                .Build();

            var planner = new PlannerBuilder()
                .WithTodos(todo)
                .WithOwnerId(new UserId(Guid.NewGuid()))
                .Build();

            await InsertPlannerAsync(planner);

            var command = new ChangeTodoDescriptionCommand(planner.Id, todo.Id, "New description");

            // Act
            var result = await Sender.Command(command);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PlannerErrors.NotFound, result.Error);

            var persistedPlanner = await FindPlannerAsync(command.PlannerId);
            Assert.NotNull(persistedPlanner);

            var persistedTodo = Assert.Single(persistedPlanner.Todos, t => t.Id == command.TodoId);
            Assert.Equal(initialDescription, persistedTodo.Description);
        }

        [Fact]
        public async Task Command_ShouldChangeTodoDescription_WhenPlannerAndTodoExists()
        {
            // Arrange
            var initialDescription = TodoDescription.Create("Initial description").Value;
            var todo = new TodoBuilder()
                .WithTodoDescription(initialDescription)
                .Build();

            var planner = new PlannerBuilder()
                .WithTodos(todo)
                .WithOwnerId(TestConstants.CurrentUserId)
                .Build();

            await InsertPlannerAsync(planner);

            var command = new ChangeTodoDescriptionCommand(planner.Id, todo.Id, "New description");

            // Act
            var result = await Sender.Command(command);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(command.TodoId, result.Value.TodoId);
            Assert.Equal(command.NewDescription, result.Value.NewDescription);

            var persistedPlanner = await FindPlannerAsync(command.PlannerId);
            Assert.NotNull(persistedPlanner);

            var persistedTodo = Assert.Single(persistedPlanner.Todos, t => t.Id == command.TodoId);
            Assert.Equal(command.NewDescription, persistedTodo.Description.Value);
        }
    }
}
