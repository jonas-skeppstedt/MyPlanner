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
            Action act = () => Todo.From(id, title);

            Assert.Throws(expectedExceptionType, act);
        }

        [Fact]
        public void From_ShouldReturnTodo_WhenAllInputsAreValid()
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
        public void ChangeTitle_ShouldThrowArgumentNullException_WhenNewTodoTitleIsNull()
        {
            // Arrange
            var todo = new TodoBuilder().Build();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => todo.ChangeTitle(null!));
        }

        [Fact]
        public void ChangeTitle_ShouldReturnNoOp_WhenNewTitleIsIdentical()
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
        public void ChangeTitle_ShouldReturnSuccess_WhenInputIsValidNewTitle()
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

        [Fact]
        public void ChangeDescription_ShouldThrowArgumentNullException_WhenNewTodoDescriptionIsNull()
        {
            // Arrange
            var todo = new TodoBuilder().Build();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => todo.ChangeDescription(null!));
        }

        [Fact]
        public void ChangeDescription_ShouldReturnNoOp_WhenNewDescriptionIsIdentical()
        {
            // Arrange
            var identicalDescription = "Identical description";
            var initialDescription = TodoDescription.Create(identicalDescription).Value;

            var todo = new TodoBuilder()
                .WithTodoDescription(initialDescription)
                .Build();

            var newDescription = TodoDescription.Create(identicalDescription).Value;

            // Act
            var result = todo.ChangeDescription(newDescription);

            // Assert
            Assert.True(result.IsNoOp);
            Assert.Equal(initialDescription, todo.Description);
        }

        [Fact]
        public void ChangeDescription_ShouldReturnSuccess_WhenInputIsValidNewDescription()
        {
            // Arrange
            var initialDescription = TodoDescription.Create("Initial description").Value;
            var newDescription = TodoDescription.Create("New description").Value;

            var todo = new TodoBuilder()
                .WithTodoDescription(initialDescription)
                .Build();

            // Act
            var result = todo.ChangeDescription(newDescription);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newDescription, todo.Description);
        }
    }
}
