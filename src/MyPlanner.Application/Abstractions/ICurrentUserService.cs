using MyPlanner.Domain.Shared;

namespace MyPlanner.Application.Abstractions
{
    public interface ICurrentUserService
    {
        Task<UserId> GetCurrentUserIdAsync();
    }
}
