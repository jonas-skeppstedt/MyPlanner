using MyPlanner.Application.Errors;
using MyPlanner.Application.Features.Planners.Commands.ChangePlannerTitle;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Planners.ValueObjects;
using MyPlanner.Domain.Shared;
using MyPlanner.Domain.TestData.Planners.Builders;
using MyPlanner.IntegrationTests.Setup;

namespace MyPlanner.IntegrationTests.Features.Planners.Commands.ChangePlannerTitle
{
    public class ChangePlannerTitleCommandIntegrationTests : IntegrationTestBase
    {
        public ChangePlannerTitleCommandIntegrationTests(IntegrationTestFixture integrationTestFixture)
            : base(integrationTestFixture)
        {
        }

        [Fact]
        public async Task Command_ShouldReturnUserNotAuthenticatedError_WhenUserIsNotAuthenticated()
        {
            // Arrange
            SetUnauthenticatedCurrentUser();

            var command = new ChangePlannerTitleCommand(PlannerId.New(), "Valid title");

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
            var initialTitle = PlannerTitle.Create("Initial title").Value;
            var planner = new PlannerBuilder()
                .WithPlannerTitle(initialTitle)
                .WithOwnerId(new UserId(Guid.NewGuid()))
                .Build();

            await InsertPlannerAsync(planner);

            var command = new ChangePlannerTitleCommand(planner.Id, "New title");

            // Act
            var result = await Sender.Command(command);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PlannerErrors.NotFound, result.Error);

            var persistedPlanner = await FindPlannerAsync(command.PlannerId);
            Assert.NotNull(persistedPlanner);
            Assert.Equal(initialTitle, persistedPlanner.Title);
        }

        [Fact]
        public async Task Command_ShouldChangePlannerTitle_WhenPlannerExists()
        {
            // Arrange
            var initialTitle = PlannerTitle.Create("Initial title").Value;
            var planner = new PlannerBuilder()
                .WithPlannerTitle(initialTitle)
                .WithOwnerId(TestConstants.CurrentUserId)
                .Build();

            await InsertPlannerAsync(planner);

            var command = new ChangePlannerTitleCommand(planner.Id, "New title");

            // Act
            var result = await Sender.Command(command);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(command.PlannerId, result.Value.PlannerId);
            Assert.Equal(command.NewTitle, result.Value.NewTitle);

            var persistedPlanner = await FindPlannerAsync(command.PlannerId);
            Assert.NotNull(persistedPlanner);
            Assert.Equal(command.NewTitle, persistedPlanner.Title.Value);
        }
    }
}
