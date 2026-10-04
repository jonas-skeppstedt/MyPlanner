using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.ValueObjects;
using MyPlanner.Domain.TestData.Planners.TheoryData;

namespace MyPlanner.Domain.UnitTests.Planners
{
    public class TodoUnitTests
    {
        [Theory]
        [ClassData(typeof(InvalidTodoFromArgumentsData))]
        public void From_ShouldThrowExpectedException_WhenAnyInputIsInvalid(
            TodoId id,
            TodoTitle title,
            Type expectedExceptionType)
        {
            // Act
            Action act = () => Todo.From(id, title);

            // Assert
            Assert.Throws(expectedExceptionType, act);
        }

        [Fact]
        public void From_ShouldReturnTodo_WhenInputsAreValid()
        {
            // Arrange
            var expectedTodoId = new TodoId(Guid.NewGuid());
            var expectedTodoTitle = TodoTitle.Create("Valid title").Value;

            // Act
            var todo = Todo.From(expectedTodoId, expectedTodoTitle);

            // Assert
            Assert.NotNull(todo);
            Assert.Equal(expectedTodoId, todo.Id);
            Assert.Equal(expectedTodoTitle, todo.Title);
            Assert.Equal(TodoDescription.Empty, todo.Description);
        }
    }
}
