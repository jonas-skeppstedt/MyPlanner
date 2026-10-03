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
        [ClassData(typeof(InvalidPlannerCreateArgumentsData))]
        public void Create_ShouldThrowExpectedException_WhenAnyInputIsInvalid(
            PlannerId id,
            UserId ownerId,
            PlannerTitle title,
            Type expectedExceptionType)
        {
            // Act
            Action act = () => Planner.Create(id, ownerId, title);

            // Assert
            Assert.Throws(expectedExceptionType, act);
        }

        [Fact]
        public void Create_ShouldReturnSuccessResult_WhenInputsAreValid()
        {
            // Arrange
            var expectedPlannerId = new PlannerId(Guid.NewGuid());
            var expectedOwnerId = new UserId(Guid.NewGuid());
            var expectedPlannerTitle = PlannerTitle.Create("Valid title").Value;

            // Act
            var result = Planner.Create(expectedPlannerId, expectedOwnerId, expectedPlannerTitle);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedPlannerId, result.Value.Id);
            Assert.Equal(expectedOwnerId, result.Value.OwnerId);
            Assert.Equal(expectedPlannerTitle, result.Value.Title);
            Assert.Empty(result.Value.Todos);
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
    }
}
