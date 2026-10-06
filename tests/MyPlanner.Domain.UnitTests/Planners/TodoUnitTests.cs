using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.ValueObjects;
using MyPlanner.Domain.TestData.Planners.Builders;
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

        [Fact]
        public void ChangeTitle_ShouldThrowExpectedException_WhenTodoTitleIsNull()
        {
            // Arrange
            var todo = new TodoBuilder().Build();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => todo.ChangeTitle(null!));
        }

        [Fact]
        public void ChangeTitle_ShouldReturnNoOpResult_WhenNewTitleIsIdentical()
        {
            // Arrange
            var identicalTitle = "Identical title";
            var initialTitle = TodoTitle.Create(identicalTitle).Value;
            var newTitle = TodoTitle.Create(identicalTitle).Value;

            var todo = new TodoBuilder()
                .WithTodoTitle(initialTitle)
                .Build();

            // Act
            var result = todo.ChangeTitle(newTitle);

            // Assert
            Assert.True(result.IsNoOp);
            Assert.Equal(initialTitle, todo.Title);
        }

        [Fact]
        public void ChangeTitle_ShouldReturnSuccessResult_WhenInputIsValidNewTitle()
        {
            // Arrange
            var initialTitle = TodoTitle.Create("Initial title").Value;
            var newTitle = TodoTitle.Create("New title").Value;

            var todo = new TodoBuilder()
                .WithTodoTitle(initialTitle)
                .Build();

            // Act
            var result = todo.ChangeTitle(newTitle);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newTitle, todo.Title);
        }
    }
}
