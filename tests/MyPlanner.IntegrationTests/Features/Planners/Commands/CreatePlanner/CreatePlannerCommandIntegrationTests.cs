using MyPlanner.Application.Errors;
using MyPlanner.Application.Features.Planners.Commands.CreatePlanner;
using MyPlanner.IntegrationTests.Setup;

namespace MyPlanner.IntegrationTests.Features.Planners.Commands.CreatePlanner
{
    public class CreatePlannerCommandIntegrationTests : IntegrationTestBase
    {
        public CreatePlannerCommandIntegrationTests(IntegrationTestFixture integrationTestFixture)
            : base(integrationTestFixture)
        {
        }

        [Fact]
        public async Task Command_ShouldReturnUserNotAuthenticatedError_WhenUserIsNotAuthenticated()
        {
            // Arrange
            SetUnauthenticatedCurrentUser();

            var command = new CreatePlannerCommand("Valid title");

            // Act
            var result = await Sender.Command(command);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(AuthenticationErrors.UserNotAuthenticated, result.Error);
        }

        [Fact]
        public async Task Command_ShouldPersistPlanner_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreatePlannerCommand("Valid title");

            // Act
            var result = await Sender.Command(command);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(default, result.Value.PlannerId);
            Assert.Equal(command.Title, result.Value.Title);

            var persistedPlanner = await FindPlannerAsync(result.Value.PlannerId);
            Assert.NotNull(persistedPlanner);
            Assert.Equal(result.Value.PlannerId, persistedPlanner.Id);
            Assert.Equal(result.Value.Title, persistedPlanner.Title.Value);
        }
    }
}
