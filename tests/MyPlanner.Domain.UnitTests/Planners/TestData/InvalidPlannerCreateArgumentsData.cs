using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.ValueObjects;
using MyPlanner.Domain.Shared;

namespace MyPlanner.Domain.UnitTests.Planners.TestData
{
    public class InvalidPlannerCreateArgumentsData : TheoryData<PlannerId, UserId, PlannerTitle, Type>
    {
        public InvalidPlannerCreateArgumentsData()
        {
            var validId = PlannerId.New();
            var validUserId = new UserId(Guid.NewGuid());
            var validPlannerTitle = PlannerTitle.Create("Valid title").Value;

            // Scenario 1: Empty PlannerId
            Add(default, validUserId, validPlannerTitle, typeof(ArgumentException));

            // Scenario 2: Empty UserId (OwnerId)
            Add(validId, default, validPlannerTitle, typeof(ArgumentException));

            // Scenario 3: Null PlannerTitle
            Add(validId, validUserId, null!, typeof(ArgumentNullException));
        }
    }
}
