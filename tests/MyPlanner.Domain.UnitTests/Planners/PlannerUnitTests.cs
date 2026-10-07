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
            // Act
            Action act = () => Planner.From(id, ownerId, title);

            // Assert
            Assert.Throws(expectedExceptionType, act);
        }

        [Fact]
        public void From_ShouldReturnPlanner_WhenInputsAreValid()
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
        public void AddTodo_ShouldThrowExpectedException_WhenTodoIsNull()
        {
            // Arrange
            var planner = new PlannerBuilder().Build();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => planner.AddTodo(null!));
        }

        [Fact]
        public void AddTodo_ShouldReturnFailureResult_WhenTodoAlreadyExistsInPlanner()
        {
            // Arrange
            var planner = new PlannerBuilder().Build();
            var todo = new TodoBuilder().Build();
            _ = planner.AddTodo(todo);

            // Act
            var result = planner.AddTodo(todo);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PlannerErrors.TodoAlreadyExists, result.Error);
            Assert.Single(planner.Todos, t => t == todo);
        }

        [Fact]
        public void AddTodo_ShouldReturnSuccessResult_WhenInputIsValid()
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
        public void RemoveTodo_ShouldThrowExpectedException_WhenTodoIdIsEmpty()
        {
            // Arrange
            var planner = new PlannerBuilder().Build();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => planner.RemoveTodo(default));
        }

        [Fact]
        public void RemoveTodo_ShouldReturnFailureResult_WhenTodoDoesNotExist()
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
        public void RemoveTodo_ShouldReturnSuccessResult_WhenTodoExists()
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
        public void ChangeTitle_ShouldThrowExpectedException_WhenNewTitleIsNull()
        {
            // Arrange
            var planner = new PlannerBuilder().Build();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => planner.ChangeTitle(null!));
        }

        [Fact]
        public void ChangeTitle_ShouldReturnNoOpResult_WhenNewTitleIsIdentical()
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
        public void ChangeTitle_ShouldReturnSuccessResult_WhenInputIsValidNewTitle()
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
        public void Delete_ShouldThrowExpectedException_WhenRequestedByUserIdIsEmpty()
        {
            //Arrange
            var planner = new PlannerBuilder().Build();

            //Act & Assert
            Assert.Throws<ArgumentException>(() => planner.Delete(default));

        }

        [Fact]
        public void Delete_ShouldReturnFailureResult_WhenRequestedByUserIdIsNotOwnerId()
        {
            //Arrange
            var planner = new PlannerBuilder()
                .WithOwnerId(new UserId(Guid.NewGuid()))
                .Build();

            var notOwnerId = new UserId(Guid.NewGuid());

            //Act
            var result = planner.Delete(notOwnerId);

            //Assert
            Assert.True(result.IsFailure);
            Assert.Equal(PlannerErrors.OnlyOwnerCanDelete, result.Error);
        }

        [Fact]
        public void Delete_ShouldReturnSuccessResult_WhenRequestedByUserIdIsOwnerId()
        {
            //Arrange
            var planner = new PlannerBuilder()
                .WithOwnerId(new UserId(Guid.NewGuid()))
                .Build();

            //Act
            var result = planner.Delete(planner.OwnerId);

            //Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void ChangeTodoTitle_ShouldThrowExpectedException_WhenTodoIdIsEmpty()
        {
            // Arrange
            var planner = new PlannerBuilder().Build();
            var newTitle = TodoTitle.Create("Valid title").Value;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => planner.ChangeTodoTitle(default, newTitle));
        }

        [Fact]
        public void ChangeTodoTitle_ShouldReturnFailureResult_WhenTodoDoesNotExist()
        {
            // Arrange
            var planner = new PlannerBuilder().Build();
            var newTitle = TodoTitle.Create("New title").Value;

            // Act
            var result = planner.ChangeTodoTitle(TodoId.New(), newTitle);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(TodoErrors.NotFound, result.Error);
        }

        [Fact]
        public void ChangeTodoTitle_ShouldReturnSuccessResult_WhenInputsAreValid()
        {
            // Arrange
            var initialTitle = TodoTitle.Create("Initial title").Value;

            var todo = new TodoBuilder()
                .WithTodoTitle(initialTitle)
                .Build();

            var planner = new PlannerBuilder()
                .WithTodos(todo)
                .Build();

            var newTitle = TodoTitle.Create("New title").Value;

            // Act
            var result = planner.ChangeTodoTitle(todo.Id, newTitle);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void ChangeTodoDescription_ShouldThrowExpectedException_WhenTodoIdIsEmpty()
        {
            // Arrange
            var planner = new PlannerBuilder().Build();
            var newDescription = TodoDescription.Create("New description").Value;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => planner.ChangeTodoDescription(default, newDescription));
        }

        [Fact]
        public void ChangeTodoDescription_ShouldReturnFailureResult_WhenTodoDoesNotExist()
        {
            // Arrange
            var planner = new PlannerBuilder().Build();
            var newDescription = TodoDescription.Create("New description").Value;

            // Act
            var result = planner.ChangeTodoDescription(TodoId.New(), newDescription);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal(TodoErrors.NotFound, result.Error);
        }

        [Fact]
        public void ChangeTodoDescription_ShouldReturnSuccessResult_WhenInputsAreValid()
        {
            // Arrange
            var initialDescription = TodoDescription.Create("Initial description").Value;

            var todo = new TodoBuilder()
                .WithTodoDescription(initialDescription)
                .Build();

            var planner = new PlannerBuilder()
                .WithTodos(todo)
                .Build();

            var newDescription = TodoDescription.Create("New description").Value;

            // Act
            var result = planner.ChangeTodoDescription(todo.Id, newDescription);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
