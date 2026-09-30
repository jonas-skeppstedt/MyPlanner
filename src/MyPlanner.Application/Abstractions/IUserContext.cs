using MyPlanner.Domain.Shared;

namespace MyPlanner.Application.Abstractions
{
    public interface IUserContext
    {
        UserId UserId { get; }

        void SetUserId(UserId userId);
    }
}
