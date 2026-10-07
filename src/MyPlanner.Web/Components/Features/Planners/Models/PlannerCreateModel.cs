using Microsoft.Build.Framework;

namespace MyPlanner.Web.Components.Features.Planners.Models
{
    public class PlannerCreateModel
    {
        [Required]
        public string Title { get; set; } = default!;
    }
}
