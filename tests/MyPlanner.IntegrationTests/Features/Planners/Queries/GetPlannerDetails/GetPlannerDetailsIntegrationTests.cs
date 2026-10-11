using MyPlanner.Application.Errors;
using MyPlanner.Application.Features.Planners.Queries.GetPlannerDetails;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Shared;
using MyPlanner.Domain.TestData.Planners.Builders;
using MyPlanner.IntegrationTests.Setup;

namespace MyPlanner.IntegrationTests.Features.Planners.Queries.GetPlannerDetails
{
    public class GetPlannerDetailsIntegrationTests : IntegrationTestBase
    {
        public GetPlannerDetailsIntegrationTests(IntegrationTestFixture integrationTestFixture)
            : base(integrationTestFixture)
        {
        }

        [Fact]
        public async Task Query_ShouldReturnUserNotAuthenticatedError_WhenUserIsNotAuthenticated()
        {
            // Arrange
            SetUnauthenticatedCurrentUser();

            var query = new GetPlannerDetailsQuery(PlannerId.New());

            // Act
            var result = await Sender.Query(query);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(AuthenticationErrors.UserNotAuthenticated, result.Error);
        }

        [Fact]
        public async Task Query_ShouldReturnPlannerNotFoundError_WhenPlannerDoesNotExist()
        {
            // Arrange
            var query = new GetPlannerDetailsQuery(PlannerId.New());

            // Act
            var result = await Sender.Query(query);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PlannerErrors.NotFound, result.Error);
        }

        [Fact]
        public async Task Query_ShouldReturnPlannerNotFoundError_WhenPlannerExistsButCurrentUserIsNotOwner()
        {
            // Arrange
            var planner = new PlannerBuilder()
                .WithOwnerId(new UserId(Guid.NewGuid()))
                .Build();

            await InsertPlannerAsync(planner);

            var query = new GetPlannerDetailsQuery(planner.Id);

            // Act
            var result = await Sender.Query(query);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PlannerErrors.NotFound, result.Error);
        }

        [Fact]
        public async Task Query_ShouldReturnPlannerDetailsDto_WhenPlannerExistsAndCurrentUserIsOwner()
        {
            // Arrange
            var planner = new PlannerBuilder()
                .WithOwnerId(TestConstants.CurrentUserId)
                .Build();

            await InsertPlannerAsync(planner);

            var query = new GetPlannerDetailsQuery(planner.Id);

            // Act
            var result = await Sender.Query(query);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(planner.Id, result.Value.PlannerId);
        }
    }
}
