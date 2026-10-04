using MyPlanner.Domain.Planners;

namespace MyPlanner.Web.Components.Features.Planners.Models
{
    public class TodoDetailsModel
    {
        public TodoId Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public TodoDetailsModel(TodoId id, string title, string description)
        {
            Id = id;
            Title = title;
            Description = description;
        }
    }
}
