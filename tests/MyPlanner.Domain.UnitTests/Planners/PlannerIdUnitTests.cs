using MyPlanner.Domain.Planners;

namespace MyPlanner.Domain.UnitTests.Planners
{
    public class PlannerIdUnitTests
    {
        [Fact]
        public void New_ShouldReturnNonEmptyPlannerId()
        {
            // Act
            var plannerId = PlannerId.New();

            // Assert
            Assert.NotEqual(Guid.Empty, plannerId.Value);
        }

        [Fact]
        public void ThrowIfEmpty_ShouldThrowArgumentException_WhenPlannerIdIsEmpty()
        {
            // Arrange
            PlannerId emptyPlannerId = default;
            var paramName = "plannerId";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => emptyPlannerId.ThrowIfEmpty(paramName));
        }

        [Fact]
        public void ToString_ShouldReturnRawValue()
        {
            // Arrange
            var expectedGuid = Guid.NewGuid();
            var plannerId = new PlannerId(expectedGuid);

            // Act
            var result = plannerId.ToString();

            // Assert
            Assert.Equal(expectedGuid.ToString(), result);
        }
    }
}
