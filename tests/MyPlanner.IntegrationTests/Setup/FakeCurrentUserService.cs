using MyPlanner.Application.Abstractions;
using MyPlanner.Domain.Shared;

namespace MyPlanner.IntegrationTests.Setup
{
    public class FakeCurrentUserService : ICurrentUserService
    {
        private static readonly UserId _testCurrentUserId = TestConstants.CurrentUserId;
        private UserId _userId = _testCurrentUserId;

        public Task<UserId> GetCurrentUserIdAsync()
        {
            return Task.FromResult(_userId);
        }

        public void SetUnauthenticatedUserId()
        {
            _userId = default;
        }

        public void Reset()
        {
            _userId = _testCurrentUserId;
        }
    }
}
