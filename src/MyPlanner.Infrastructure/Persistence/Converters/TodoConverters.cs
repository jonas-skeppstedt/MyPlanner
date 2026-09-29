using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.ValueObjects;

namespace MyPlanner.Infrastructure.Persistence.Converters
{
    internal class TodoIdConverter()
        : ValueConverter<TodoId, Guid>(id => id.Value, value => new TodoId(value));

    internal class TodoTitleConverter()
        : ValueConverter<TodoTitle, string>(title => title.Value, value => TodoTitle.Create(value).Value);

    internal class TodoDescriptionConverter()
        : ValueConverter<TodoDescription, string>(description => description.Value, value => TodoDescription.Create(value).Value);
}
