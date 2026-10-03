using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.ValueObjects;
using MyPlanner.Domain.Shared;
using MyPlanner.Domain.TestData.Planners.TheoryData;

namespace MyPlanner.Domain.UnitTests.Planners
{
    public class PlannerUnitTests
    {
        [Theory]
        [ClassData(typeof(InvalidPlannerCreateArgumentsData))]
        public void Create_ShouldThrowExpectedException_WhenAnyInputIsInvalid(
            PlannerId id,
            UserId ownerId,
            PlannerTitle title,
            Type expectedExceptionType)
        {
            // Act
            Action act = () => Planner.Create(id, ownerId, title);

            // Assert
            Assert.Throws(expectedExceptionType, act);
        }

        [Fact]
        public void Create_ShouldReturnSuccessResult_WhenInputsAreValid()
        {
            // Arrange
            var expectedPlannerId = new PlannerId(Guid.NewGuid());
            var expectedOwnerId = new UserId(Guid.NewGuid());
            var expectedPlannerTitle = PlannerTitle.Create("Valid title").Value;

            // Act
            var result = Planner.Create(expectedPlannerId, expectedOwnerId, expectedPlannerTitle);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedPlannerId, result.Value.Id);
            Assert.Equal(expectedOwnerId, result.Value.OwnerId);
            Assert.Equal(expectedPlannerTitle, result.Value.Title);
            Assert.Empty(result.Value.Todos);
        }
    }
}
