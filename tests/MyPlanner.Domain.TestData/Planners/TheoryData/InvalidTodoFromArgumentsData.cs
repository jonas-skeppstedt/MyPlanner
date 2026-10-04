using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.ValueObjects;
using Xunit;

namespace MyPlanner.Domain.TestData.Planners.TheoryData
{
    public class InvalidTodoFromArgumentsData : TheoryData<TodoId, TodoTitle, Type>
    {
        public InvalidTodoFromArgumentsData()
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
