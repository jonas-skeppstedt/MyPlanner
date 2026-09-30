using Microsoft.AspNetCore.Components.Authorization;
using MyPlanner.Application.Abstractions;
using MyPlanner.Domain.Shared;
using System.Security.Claims;

namespace MyPlanner.Web.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public CurrentUserService(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<UserId> GetCurrentUserIdAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

            if (authState.User.Identity?.IsAuthenticated == true)
            {
                var rawUserId = authState.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (UserId.TryParse(rawUserId, out var userId))
                {
                    return userId;
                }
            }

            return default;
        }
    }
}
