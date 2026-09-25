using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.ValueObjects;

namespace MyPlanner.Domain.UnitTests.Planners.TestData
{
    public class InvalidTodoCreateArgumentsData : TheoryData<TodoId, TodoTitle, Type>
    {
        public InvalidTodoCreateArgumentsData()
        {
            var validId = new TodoId(Guid.NewGuid());
            var validTitle = TodoTitle.Create("Valid title").Value;

            // Scenario 1: Empty TodoId
            Add(default, validTitle, typeof(ArgumentException));

            // Scenario 2: Null TodoTitle
            Add(validId, null!, typeof(ArgumentNullException));
        }
    }
}
