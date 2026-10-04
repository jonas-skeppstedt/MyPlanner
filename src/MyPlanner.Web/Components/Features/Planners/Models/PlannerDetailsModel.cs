using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Shared;

namespace MyPlanner.Web.Components.Features.Planners.Models
{
    public class PlannerDetailsModel
    {
        public PlannerId Id { get; set; }
        public UserId OwnerId { get; set; }
        public string Title { get; set; }
        public List<TodoDetailsModel> Todos { get; set; }

        public PlannerDetailsModel(PlannerId id, UserId ownerId, string title, List<TodoDetailsModel> todos)
        {
            Id = id;
            OwnerId = ownerId;
            Title = title;
            Todos = todos;
        }
    }
}
