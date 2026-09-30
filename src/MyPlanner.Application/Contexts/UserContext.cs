using MyPlanner.Application.Abstractions;
using MyPlanner.Domain.Shared;

namespace MyPlanner.Application.Contexts
{
    internal class UserContext : IUserContext
    {
        private bool _hasBeenSet;

        public UserId UserId { get; private set; }

        public void SetUserId(UserId userId)
        {
            if (_hasBeenSet)
            {
                throw new InvalidOperationException($"{nameof(UserId)} has already been set.");
            }

            UserId = userId;
            _hasBeenSet = true;
        }
    }
}
