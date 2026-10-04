using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.ValueObjects;

namespace MyPlanner.Domain.TestData.Planners.Builders
{
    public class TodoBuilder
    {
        private TodoId _id = TodoId.New();
        private TodoTitle _title = TodoTitle.Create("Todo title").Value;

        public TodoBuilder WithTodoId(TodoId id)
        {
            _id = id;
            return this;
        }

        public TodoBuilder WithTodoTitle(TodoTitle title)
        {
            _title = title;
            return this;
        }

        public Todo Build()
        {
            return Todo.From(_id, _title);
        }
    }
}
