using MyPlanner.Domain.Planners;
using System.ComponentModel.DataAnnotations;

namespace MyPlanner.Web.Components.Features.Planners.Models
{
    public class PlannerChangeTitleModel
    {
        public PlannerId PlannerId { get; set; }

        [Required]
        [StringLength(50)]
        public string NewTitle { get; set; } = default!;
    }
}
