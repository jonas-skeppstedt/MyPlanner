using System.ComponentModel.DataAnnotations;

namespace MyPlanner.Web.Components.Features.Planners.Models
{
    public class PlannerChangeTitleModel
    {
        [Required]
        [StringLength(50)]
        public string NewTitle { get; set; } = default!;
    }
}
