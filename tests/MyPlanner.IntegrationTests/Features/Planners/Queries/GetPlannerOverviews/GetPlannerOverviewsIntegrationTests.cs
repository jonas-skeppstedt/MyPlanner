using MyPlanner.Application.Features.Planners.Queries.GetPlannerOverviews;
using MyPlanner.Domain.Shared;
using MyPlanner.Domain.TestData.Planners.Builders;
using MyPlanner.IntegrationTests.Setup;

namespace MyPlanner.IntegrationTests.Features.Planners.Queries.GetPlannerOverviews
{
    public class GetPlannerOverviewsIntegrationTests : IntegrationTestBase
    {
        public GetPlannerOverviewsIntegrationTests(IntegrationTestFixture integrationTestFixture)
            : base(integrationTestFixture)
        {
        }

        [Fact]
        public async Task Query_ShouldReturnEmptyList_WhenCurrentUserHasNoPlanners()
        {
            // Arrange
            var query = new GetPlannerOverviewsQuery();

            // Act
            var result = await Sender.Query(query);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task Query_ShouldReturnEmptyList_WhenCurrentUserIsNotOwner()
        {
            // Arrange
            var planner = new PlannerBuilder()
                .WithOwnerId(new UserId(Guid.NewGuid()))
                .Build();

            await InsertPlannerAsync(planner);

            var query = new GetPlannerOverviewsQuery();

            // Act
            var result = await Sender.Query(query);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);
        }

        [Fact]
        public async Task Query_ShouldReturnPlannerOverviewDtos_WhenCurrentUserHasPlanners()
        {
            // Arrange
            var planner1 = new PlannerBuilder()
                .WithOwnerId(TestConstants.CurrentUserId)
                .Build();

            var planner2 = new PlannerBuilder()
                .WithOwnerId(TestConstants.CurrentUserId)
                .Build();

            await InsertPlannersRangeAsync(planner1, planner2);

            var query = new GetPlannerOverviewsQuery();

            // Act
            var result = await Sender.Query(query);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(2, result.Value.Count);

            var returnedIds = result.Value
                .Select(p => p.PlannerId)
                .ToList();

            Assert.Contains(planner1.Id, returnedIds);
            Assert.Contains(planner2.Id, returnedIds);
        }
    }
}
