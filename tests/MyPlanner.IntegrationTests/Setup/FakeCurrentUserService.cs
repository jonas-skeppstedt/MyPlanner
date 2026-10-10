using MyPlanner.Application.Abstractions;
using MyPlanner.Domain.Shared;

namespace MyPlanner.IntegrationTests.Setup
{
    public class FakeCurrentUserService : ICurrentUserService
    {
        public Task<UserId> GetCurrentUserIdAsync()
        {
            return Task.FromResult(TestConstants.CurrentUserId);
        }
    }
}
