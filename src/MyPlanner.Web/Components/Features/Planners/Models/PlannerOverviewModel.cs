using MyPlanner.Domain.Planners;

namespace MyPlanner.Web.Components.Features.Planners.Models
{
    public class PlannerOverviewModel
    {
        public PlannerId Id { get; set; }
        public string Title { get; set; }

        public PlannerOverviewModel(PlannerId id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
