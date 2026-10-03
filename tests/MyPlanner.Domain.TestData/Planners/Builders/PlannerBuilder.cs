using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.ValueObjects;
using MyPlanner.Domain.Shared;

namespace MyPlanner.Domain.TestData.Planners.Builders
{
    public class PlannerBuilder
    {
        private PlannerId _id;
        private UserId _ownerId;
        private PlannerTitle _title = PlannerTitle.Create("Planner title").Value;

        public PlannerBuilder WithPlannerId(PlannerId id)
        {
            _id = id;
            return this;
        }

        public PlannerBuilder WithOwnerId(UserId ownerId)
        {
            _ownerId = ownerId;
            return this;
        }

        public PlannerBuilder WithPlannerTitle(PlannerTitle title)
        {
            _title = title;
            return this;
        }

        public Planner Build()
        {
            if (_id.Value == Guid.Empty)
            {
                throw new InvalidOperationException(
                    "Cannot build Planner without a PlannerId. Call WithPlannerId() first.");
            }

            if (_ownerId.Value == Guid.Empty)
            {
                throw new InvalidOperationException(
                    "Cannot build Planner without a OwnerId. Call WithOwnerId() first.");
            }

            return Planner.Create(_id, _ownerId, _title).Value;
        }
    }
}
