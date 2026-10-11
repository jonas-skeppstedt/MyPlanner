using MyPlanner.Application.Errors;
using MyPlanner.Application.Features.Planners.Commands.RemoveTodo;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Shared;
using MyPlanner.Domain.TestData.Planners.Builders;
using MyPlanner.IntegrationTests.Setup;

namespace MyPlanner.IntegrationTests.Features.Planners.Commands.RemoveTodo
{
    public class RemoveTodoCommandIntegrationTests : IntegrationTestBase
    {
        public RemoveTodoCommandIntegrationTests(IntegrationTestFixture integrationTestFixture)
            : base(integrationTestFixture)
        {
        }

        [Fact]
        public async Task Command_ShouldReturnUserNotAuthenticatedError_WhenUserIsNotAuthenticated()
        {
            // Arrange
            SetUnauthenticatedCurrentUser();

            var command = new RemoveTodoCommand(PlannerId.New(), TodoId.New());

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
            var todo = new TodoBuilder().Build();
            var planner = new PlannerBuilder()
                .WithTodos(todo)
                .WithOwnerId(new UserId(Guid.NewGuid()))
                .Build();

            await InsertPlannerAsync(planner);

            var command = new RemoveTodoCommand(planner.Id, todo.Id);

            // Act
            var result = await Sender.Command(command);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PlannerErrors.NotFound, result.Error);

            var persistedPlanner = await FindPlannerAsync(command.PlannerId);
            Assert.NotNull(persistedPlanner);
            Assert.Single(persistedPlanner.Todos, t => t.Id == command.TodoId);
        }

        [Fact]
        public async Task Command_ShouldRemoveTodo_WhenPlannerAndTodoExists()
        {
            // Arrange
            var todo = new TodoBuilder().Build();
            var planner = new PlannerBuilder()
                .WithTodos(todo)
                .WithOwnerId(TestConstants.CurrentUserId)
                .Build();

            await InsertPlannerAsync(planner);

            var command = new RemoveTodoCommand(planner.Id, todo.Id);

            // Act
            var result = await Sender.Command(command);

            // Assert
            Assert.True(result.IsSuccess);

            var persistedPlanner = await FindPlannerAsync(command.PlannerId);
            Assert.NotNull(persistedPlanner);
            Assert.Empty(persistedPlanner.Todos);
        }
    }
}
