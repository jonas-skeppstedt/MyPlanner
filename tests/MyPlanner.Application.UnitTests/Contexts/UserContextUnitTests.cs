using MyPlanner.Application.Contexts;
using MyPlanner.Domain.Shared;

namespace MyPlanner.Application.UnitTests.Contexts
{
    public class UserContextUnitTests
    {
        private readonly UserContext _userContext = new();

        [Fact]
        public void SetUserId_ShouldSetUserId_WhenNotSet()
        {
            // Arrange
            var expectedUserId = new UserId(Guid.NewGuid());
            _userContext.SetUserId(expectedUserId);

            // Act
            var actual = _userContext.UserId;

            // Assert
            Assert.Equal(expectedUserId, actual);
        }

        [Fact]
        public void SetUserId_ShouldThrowExpectedException_WhenUserIsAlreadySet()
        {
            // Arrange
            var userId = new UserId(Guid.NewGuid());
            _userContext.SetUserId(userId);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _userContext.SetUserId(userId));
        }
    }
}
