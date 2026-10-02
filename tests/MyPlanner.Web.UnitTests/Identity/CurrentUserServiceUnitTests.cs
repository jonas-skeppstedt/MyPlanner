using Microsoft.AspNetCore.Components.Authorization;
using Moq;
using MyPlanner.Domain.Shared;
using MyPlanner.Web.Identity;
using System.Security.Claims;

namespace MyPlanner.Web.UnitTests.Identity
{
    public class CurrentUserServiceUnitTests
    {
        private readonly CurrentUserService _sut;
        private readonly Mock<AuthenticationStateProvider> _authenticationStateProviderMock = new();

        public CurrentUserServiceUnitTests()
        {
            _sut = new CurrentUserService(_authenticationStateProviderMock.Object);
        }

        private AuthenticationState ArrangeAuthenticatedUser(UserId userId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);
            var authState = new AuthenticationState(principal);

            return authState;
        }

        private AuthenticationState ArrangeUnauthenticatedUser()
        {
            var identity = new ClaimsIdentity();
            var principal = new ClaimsPrincipal(identity);
            var authState = new AuthenticationState(principal);

            return authState;
        }

        [Fact]
        public async Task GetCurrentUserIdAsync_ShouldReturnValidUserId_WhenCurrentUserIsAuthenticated()
        {
            // Arrange
            var expectedUserId = new UserId(Guid.NewGuid());
            var authState = ArrangeAuthenticatedUser(expectedUserId);

            _authenticationStateProviderMock
                .Setup(x => x.GetAuthenticationStateAsync())
                .ReturnsAsync(authState);

            // Act
            var actual = await _sut.GetCurrentUserIdAsync();

            // Assert
            Assert.Equal(expectedUserId, actual);
        }

        [Fact]
        public async Task GetCurrentUserIdAsync_ShouldReturnDefault_WhenCurrentUserIsNotAuthenticated()
        {
            // Arrange
            UserId expectedDefaultUserId = default;
            var authState = ArrangeUnauthenticatedUser();

            _authenticationStateProviderMock
                .Setup(x => x.GetAuthenticationStateAsync())
                .ReturnsAsync(authState);

            // Act
            var actual = await _sut.GetCurrentUserIdAsync();

            // Assert
            Assert.Equal(expectedDefaultUserId, actual);
        }
    }
}
