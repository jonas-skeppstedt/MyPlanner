using MyPlanner.Domain.Planners;
using MyPlanner.Domain.Planners.Errors;
using MyPlanner.Domain.Planners.ValueObjects;
using MyPlanner.Domain.Shared;
using MyPlanner.Domain.TestData.Planners.Builders;
using MyPlanner.Domain.TestData.Planners.TheoryData;

namespace MyPlanner.Domain.UnitTests.Planners
{
    public class PlannerUnitTests
    {
        [Theory]
        [ClassData(typeof(InvalidPlannerFromArgumentsData))]
        public void From_ShouldThrowExpectedException_WhenAnyInputIsInvalid(
            PlannerId id,
            UserId ownerId,
            PlannerTitle title,
            Type expectedExceptionType)
        {
            Action act = () => Planner.From(id, ownerId, title);

            Assert.Throws(expectedExceptionType, act);
        }

        [Fact]
        public void From_ShouldReturnPlanner_WhenAllInputsAreValid()
        {
            // Arrange
            var expectedPlannerId = new PlannerId(Guid.NewGuid());
            var expectedOwnerId = new UserId(Guid.NewGuid());
            var expectedPlannerTitle = PlannerTitle.Create("Valid title").Value;

            // Act
            var planner = Planner.From(expectedPlannerId, expectedOwnerId, expectedPlannerTitle);

            // Assert
            Assert.NotNull(planner);
            Assert.Equal(expectedPlannerId, planner.Id);
            Assert.Equal(expectedOwnerId, planner.OwnerId);
            Assert.Equal(expectedPlannerTitle, planner.Title);
            Assert.Empty(planner.Todos);
        }

        [Fact]
        public void AddTodo_ShouldThrowArgumentNullException_WhenTodoIsNull()
        {
            // Arrange
            var planner = new PlannerBuilder().Build();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => planner.AddTodo(null!));
        }

        [Fact]
        public void AddTodo_ShouldReturnFailureWithTodoAlreadyExistsError_WhenTodoAlreadyExistsInPlanner()
        {
            // Arrange
            var todo = new TodoBuilder().Build();
            var planner = new PlannerBuilder()
                .WithTodos(todo)
                .Build();

            // Act
            var result = planner.AddTodo(todo);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PlannerErrors.TodoAlreadyExists, result.Error);
            Assert.Single(planner.Todos, t => t == todo);
        }

        [Fact]
        public void AddTodo_ShouldReturnSuccess_WhenTodoIsUnique()
        {
            // Arrange
            var todo = new TodoBuilder().Build();
            var planner = new PlannerBuilder().Build();

            // Act
            var result = planner.AddTodo(todo);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(planner.Todos, t => t == todo);
        }

        [Fact]
        public void RemoveTodo_ShouldThrowArgumentException_WhenTodoIdIsEmpty()
        {
            // Arrange
            var planner = new PlannerBuilder().Build();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => planner.RemoveTodo(default));
        }

        [Fact]
        public void RemoveTodo_ShouldReturnFailureWithTodoNotFoundError_WhenTodoDoesNotExist()
        {
            // Arrange
            var todo = new TodoBuilder().Build();
            var planner = new PlannerBuilder()
                .WithTodos(todo)
                .Build();

            // Act
            var result = planner.RemoveTodo(TodoId.New());

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(TodoErrors.NotFound, result.Error);
            Assert.Single(planner.Todos, t => t == todo);
        }

        [Fact]
        public void RemoveTodo_ShouldReturnSuccess_WhenTodoExists()
        {
            // Arrange
            var todo = new TodoBuilder().Build();
            var planner = new PlannerBuilder()
                .WithTodos(todo)
                .Build();

            // Act
            var result = planner.RemoveTodo(todo.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Empty(planner.Todos);
        }

        [Fact]
        public void ChangeTitle_ShouldThrowArgumentNullException_WhenNewTitleIsNull()
        {
            // Arrange
            var planner = new PlannerBuilder().Build();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => planner.ChangeTitle(null!));
        }

        [Fact]
        public void ChangeTitle_ShouldReturnNoOp_WhenNewTitleIsIdentical()
        {
            // Arrange
            var identicalTitle = "Identical title";
            var initialTitle = PlannerTitle.Create(identicalTitle).Value;
            var newTitle = PlannerTitle.Create(identicalTitle).Value;

            var planner = new PlannerBuilder()
                .WithPlannerTitle(initialTitle)
                .Build();

            // Act
            var result = planner.ChangeTitle(newTitle);

            // Assert
            Assert.True(result.IsNoOp);
            Assert.Equal(initialTitle, planner.Title);
        }

        [Fact]
        public void ChangeTitle_ShouldReturnSuccess_WhenInputIsValidNewTitle()
        {
            // Arrange
            var initialTitle = PlannerTitle.Create("Initial title").Value;
            var newTitle = PlannerTitle.Create("New title").Value;

            var planner = new PlannerBuilder()
                .WithPlannerTitle(initialTitle)
                .Build();

            // Act
            var result = planner.ChangeTitle(newTitle);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(newTitle, planner.Title);
        }

        [Fact]
        public void Delete_ShouldThrowArgumentException_WhenRequestedByUserIdIsEmpty()
        {
            // Arrange
            var planner = new PlannerBuilder().Build();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => planner.Delete(default));

        }

        [Fact]
        public void Delete_ShouldReturnFailureWithPlannerOnlyOwnerCanDeleteError_WhenRequestedByUserIdIsNotOwnerId()
        {
            // Arrange
            var notOwnerId = new UserId(Guid.NewGuid());
            var ownerId = new UserId(Guid.NewGuid());

            var planner = new PlannerBuilder()
                .WithOwnerId(ownerId)
                .Build();

            // Act
            var result = planner.Delete(notOwnerId);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PlannerErrors.OnlyOwnerCanDelete, result.Error);
        }

        [Fact]
        public void Delete_ShouldReturnSuccess_WhenRequestedByUserIdIsOwnerId()
        {
            // Arrange
            var ownerId = new UserId(Guid.NewGuid());

            var planner = new PlannerBuilder()
                .WithOwnerId(ownerId)
                .Build();

            // Act
            var result = planner.Delete(ownerId);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void ChangeTodoTitle_ShouldThrowArgumentException_WhenTodoIdIsEmpty()
        {
            // Arrange
            var newTitle = TodoTitle.Create("New title").Value;

            var planner = new PlannerBuilder().Build();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => planner.ChangeTodoTitle(default, newTitle));
        }

        [Fact]
        public void ChangeTodoTitle_ShouldReturnFailureWithTodoNotFoundError_WhenTodoDoesNotExist()
        {
            // Arrange
            var newTitle = TodoTitle.Create("New title").Value;

            var planner = new PlannerBuilder().Build();

            // Act
            var result = planner.ChangeTodoTitle(TodoId.New(), newTitle);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(TodoErrors.NotFound, result.Error);
        }

        [Fact]
        public void ChangeTodoTitle_ShouldReturnSuccess_WhenTodoExistsAndNewTitleIsValid()
        {
            // Arrange
            var initialTitle = TodoTitle.Create("Initial title").Value;
            var newTitle = TodoTitle.Create("New title").Value;

            var todo = new TodoBuilder()
                .WithTodoTitle(initialTitle)
                .Build();

            var planner = new PlannerBuilder()
                .WithTodos(todo)
                .Build();

            // Act
            var result = planner.ChangeTodoTitle(todo.Id, newTitle);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void ChangeTodoDescription_ShouldThrowArgumentException_WhenTodoIdIsEmpty()
        {
            // Arrange
            var newDescription = TodoDescription.Create("New description").Value;

            var planner = new PlannerBuilder().Build();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => planner.ChangeTodoDescription(default, newDescription));
        }

        [Fact]
        public void ChangeTodoDescription_ShouldReturnFailureWithTodoNotFoundError_WhenTodoDoesNotExist()
        {
            // Arrange
            var newDescription = TodoDescription.Create("New description").Value;

            var planner = new PlannerBuilder().Build();

            // Act
            var result = planner.ChangeTodoDescription(TodoId.New(), newDescription);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(TodoErrors.NotFound, result.Error);
        }

        [Fact]
        public void ChangeTodoDescription_ShouldReturnSuccess_WhenTodoExistsAndNewDescriptionIsValid()
        {
            // Arrange
            var initialDescription = TodoDescription.Create("Initial description").Value;
            var newDescription = TodoDescription.Create("New description").Value;

            var todo = new TodoBuilder()
                .WithTodoDescription(initialDescription)
                .Build();

            var planner = new PlannerBuilder()
                .WithTodos(todo)
                .Build();

            // Act
            var result = planner.ChangeTodoDescription(todo.Id, newDescription);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
