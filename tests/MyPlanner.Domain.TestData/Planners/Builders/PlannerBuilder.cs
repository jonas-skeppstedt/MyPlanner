using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.ValueObjects;
using MyPlanner.Domain.Shared;

namespace MyPlanner.Domain.TestData.Planners.Builders
{
    public class PlannerBuilder
    {
        private PlannerId _id = PlannerId.New();
        private UserId _ownerId = new UserId(Guid.NewGuid());
        private PlannerTitle _title = PlannerTitle.Create("Planner title").Value;
        private List<Todo> _todos = new();

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

        public PlannerBuilder WithTodos(params Todo[] todos)
        {
            _todos.AddRange(todos);
            return this;
        }

        public Planner Build()
        {
            var planner = Planner.From(_id, _ownerId, _title);

            foreach (var todo in _todos)
            {
                planner.AddTodo(todo);
            }

            return planner;
        }
    }
}
