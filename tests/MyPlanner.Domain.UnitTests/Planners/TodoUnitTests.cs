using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.ValueObjects;
using MyPlanner.Domain.UnitTests.Planners.TestData;

namespace MyPlanner.Domain.UnitTests.Planners
{
    public class TodoUnitTests
    {
        [Theory]
        [ClassData(typeof(InvalidTodoCreateArgumentsData))]
        public void Create_ShouldThrowExpectedException_WhenAnyInputIsInvalid(
            TodoId id,
            TodoTitle title,
            Type expectedExceptionType)
        {
            // Act
            Action act = () => Todo.Create(id, title);

            // Assert
            Assert.Throws(expectedExceptionType, act);
        }

        [Fact]
        public void Create_ShouldReturnSuccessResult_WhenInputsAreValid()
        {
            // Arrange
            var expectedTodoId = new TodoId(Guid.NewGuid());
            var expectedTodoTitle = TodoTitle.Create("Valid title").Value;

            // Act
            var result = Todo.Create(expectedTodoId, expectedTodoTitle);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedTodoId, result.Value.Id);
            Assert.Equal(expectedTodoTitle, result.Value.Title);
            Assert.Equal(TodoDescription.Empty, result.Value.Description);
        }
    }
}
