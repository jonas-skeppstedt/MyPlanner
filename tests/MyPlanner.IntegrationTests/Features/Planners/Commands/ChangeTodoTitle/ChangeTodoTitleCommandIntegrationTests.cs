using MyPlanner.Application.Features.Planners.Commands.ChangeTodoTitle;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Planners.ValueObjects;
using MyPlanner.Domain.Shared;
using MyPlanner.Domain.TestData.Planners.Builders;
using MyPlanner.IntegrationTests.Setup;

namespace MyPlanner.IntegrationTests.Features.Planners.Commands.ChangeTodoTitle
{
    public class ChangeTodoTitleCommandIntegrationTests : IntegrationTestBase
    {
        public ChangeTodoTitleCommandIntegrationTests(IntegrationTestFixture integrationTestFixture)
            : base(integrationTestFixture)
        {
        }

        [Fact]
        public async Task Command_ShouldReturnPlannerNotFoundError_WhenPlannerExistsButCurrentUserIsNotOwner()
        {
            // Arrange
            var initialTitle = TodoTitle.Create("Initial title").Value;
            var todo = new TodoBuilder()
                .WithTodoTitle(initialTitle)
                .Build();

            var planner = new PlannerBuilder()
                .WithTodos(todo)
                .WithOwnerId(new UserId(Guid.NewGuid()))
                .Build();

            await InsertPlannerAsync(planner);

            var command = new ChangeTodoTitleCommand(planner.Id, todo.Id, "New title");

            // Act
            var result = await Sender.Command(command);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PlannerErrors.NotFound, result.Error);

            var persistedPlanner = await FindPlannerAsync(command.PlannerId);
            Assert.NotNull(persistedPlanner);

            var persistedTodo = Assert.Single(persistedPlanner.Todos, t => t.Id == command.TodoId);
            Assert.Equal(initialTitle, persistedTodo.Title);
        }

        [Fact]
        public async Task Command_ShouldChangeTodoTitle_WhenPlannerAndTodoExists()
        {
            // Arrange
            var initialTitle = TodoTitle.Create("Initial title").Value;
            var todo = new TodoBuilder()
                .WithTodoTitle(initialTitle)
                .Build();

            var planner = new PlannerBuilder()
                .WithTodos(todo)
                .WithOwnerId(TestConstants.CurrentUserId)
                .Build();

            await InsertPlannerAsync(planner);

            var command = new ChangeTodoTitleCommand(planner.Id, todo.Id, "New title");

            // Act
            var result = await Sender.Command(command);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(command.TodoId, result.Value.TodoId);
            Assert.Equal(command.NewTitle, result.Value.NewTitle);

            var persistedPlanner = await FindPlannerAsync(command.PlannerId);
            Assert.NotNull(persistedPlanner);

            var persistedTodo = Assert.Single(persistedPlanner.Todos, t => t.Id == command.TodoId);
            Assert.Equal(command.NewTitle, persistedTodo.Title.Value);
        }
    }
}
