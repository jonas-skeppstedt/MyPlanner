using MyPlanner.Application.Errors;
using MyPlanner.Application.Features.Planners.Commands.DeletePlanner;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Shared;
using MyPlanner.Domain.TestData.Planners.Builders;
using MyPlanner.IntegrationTests.Setup;

namespace MyPlanner.IntegrationTests.Features.Planners.Commands.DeletePlanner
{
    public class DeletePlannerCommandIntegrationTests : IntegrationTestBase
    {
        public DeletePlannerCommandIntegrationTests(IntegrationTestFixture integrationTestFixture)
            : base(integrationTestFixture)
        {
        }

        [Fact]
        public async Task Command_ShouldReturnUserNotAuthenticatedError_WhenUserIsNotAuthenticated()
        {
            // Arrange
            SetUnauthenticatedCurrentUser();

            var command = new DeletePlannerCommand(PlannerId.New());

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
            var planner = new PlannerBuilder()
                .WithOwnerId(new UserId(Guid.NewGuid()))
                .Build();

            await InsertPlannerAsync(planner);

            var command = new DeletePlannerCommand(planner.Id);

            // Act
            var result = await Sender.Command(command);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PlannerErrors.NotFound, result.Error);

            var persistedPlanner = await FindPlannerAsync(command.PlannerId);
            Assert.NotNull(persistedPlanner);
        }

        [Fact]
        public async Task Command_ShouldDeletePlanner_WhenPlannerExistsAndCurrentUserIsOwner()
        {
            // Arrange
            var planner = new PlannerBuilder()
                .WithOwnerId(TestConstants.CurrentUserId)
                .Build();

            await InsertPlannerAsync(planner);

            var command = new DeletePlannerCommand(planner.Id);

            // Act
            var result = await Sender.Command(command);

            // Assert
            Assert.True(result.IsSuccess);

            var deletedPlanner = await FindPlannerAsync(command.PlannerId);
            Assert.Null(deletedPlanner);
        }
    }
}
