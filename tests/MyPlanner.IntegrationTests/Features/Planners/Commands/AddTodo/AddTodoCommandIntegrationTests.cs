using MyPlanner.Application.Features.Planners.Commands.AddTodo;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Shared;
using MyPlanner.Domain.TestData.Planners.Builders;
using MyPlanner.IntegrationTests.Setup;

namespace MyPlanner.IntegrationTests.Features.Planners.Commands.AddTodo
{
    public class AddTodoCommandIntegrationTests : IntegrationTestBase
    {
        public AddTodoCommandIntegrationTests(IntegrationTestFixture integrationTestFixture)
            : base(integrationTestFixture)
        {
        }

        [Fact]
        public async Task Command_ShouldReturnPlannerNotFoundError_WhenPlannerExistsButCurrentUserIsNotOwner()
        {
            // Arrange
            var planner = new PlannerBuilder()
                .WithOwnerId(new UserId(Guid.NewGuid()))
                .Build();

            await InsertPlannerAsync(planner);

            var command = new AddTodoCommand(planner.Id, "Valid title");

            // Act
            var result = await Sender.Command(command);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PlannerErrors.NotFound, result.Error);

            var persistedPlanner = await FindPlannerAsync(command.PlannerId);
            Assert.NotNull(persistedPlanner);
            Assert.Empty(persistedPlanner.Todos);
        }

        [Fact]
        public async Task Command_ShouldAddTodo_WhenPlannerExists()
        {
            // Arrange
            var planner = new PlannerBuilder()
                .WithOwnerId(TestConstants.CurrentUserId)
                .Build();

            await InsertPlannerAsync(planner);

            var command = new AddTodoCommand(planner.Id, "Valid title");

            // Act
            var result = await Sender.Command(command);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(default, result.Value.TodoId);
            Assert.Equal(command.Title, result.Value.Title);

            var persistedPlanner = await FindPlannerAsync(command.PlannerId);
            Assert.NotNull(persistedPlanner);

            var persistedTodo = Assert.Single(persistedPlanner.Todos, t => t.Id == result.Value.TodoId);
            Assert.Equal(command.Title, persistedTodo.Title.Value);
        }
    }
}
