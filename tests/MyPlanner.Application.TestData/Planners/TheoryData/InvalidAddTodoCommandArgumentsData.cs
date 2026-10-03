using MyPlanner.Domain.Common;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Planners.ValueObjects;
using Xunit;

namespace MyPlanner.Application.TestData.Planners.TheoryData
{
    public class InvalidAddTodoCommandArgumentsData : TheoryData<string, Error>
    {
        public InvalidAddTodoCommandArgumentsData()
        {
            // Scenario 1: Title IsNullOrWhitespace
            Add(null!, TodoTitleErrors.TitleRequired);
            Add("", TodoTitleErrors.TitleRequired);
            Add("  ", TodoTitleErrors.TitleRequired);

            // Scenario 2: Title is too long
            Add(new string('x', TodoTitle.MaxLength + 1), TodoTitleErrors.TitleTooLong);
        }
    }
}
