using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.ValueObjects;

namespace MyPlanner.Domain.TestData.Planners.Builders
{
    public class TodoBuilder
    {
        private TodoId _id = TodoId.New();
        private TodoTitle _title = TodoTitle.Create("Todo title").Value;
        private TodoDescription _description = TodoDescription.Empty;

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

        public TodoBuilder WithTodoDescription(TodoDescription description)
        {
            _description = description;
            return this;
        }

        public Todo Build()
        {
            var todo = Todo.From(_id, _title);

            _ = todo.ChangeDescription(_description);

            return todo;
        }
    }
}
