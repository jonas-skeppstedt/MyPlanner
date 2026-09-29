using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.ValueObjects;

namespace MyPlanner.Infrastructure.Persistence.Converters
{
    internal class PlannerIdConverter()
        : ValueConverter<PlannerId, Guid>(id => id.Value, value => new PlannerId(value));

    internal class PlannerTitleConverter()
        : ValueConverter<PlannerTitle, string>(title => title.Value, value => PlannerTitle.Create(value).Value);
}
