using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyPlanner.Domain.Shared;

namespace MyPlanner.Infrastructure.Persistence.Converters
{
    internal class UserIdConverter()
        : ValueConverter<UserId, Guid>(id => id.Value, value => new UserId(value));
}
