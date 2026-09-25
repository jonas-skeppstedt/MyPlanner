using MyPlanner.Domain.Planners;

namespace MyPlanner.Domain.UnitTests.Planners
{
    public class TodoIdUnitTests
    {
        [Fact]
        public void New_ShouldReturnNonEmptyTodoId()
        {
            // Act
            var todoId = TodoId.New();

            // Assert
            Assert.NotEqual(Guid.Empty, todoId.Value);
        }

        [Fact]
        public void ThrowIfEmpty_ShouldThrowArgumentException_WhenTodoIdIsEmpty()
        {
            // Arrange
            TodoId emptyTodoId = default;
            var paramName = "todoId";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => emptyTodoId.ThrowIfEmpty(paramName));
        }

        [Fact]
        public void ToString_ShouldReturnRawValue()
        {
            // Arrange
            var expectedGuid = Guid.NewGuid();
            var todoId = new TodoId(expectedGuid);

            // Act
            var result = todoId.ToString();

            // Assert
            Assert.Equal(expectedGuid.ToString(), result);
        }
    }
}
