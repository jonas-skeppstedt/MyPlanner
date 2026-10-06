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
        public void Delete_ShouldThrowExpectedException_WhenRequestedByUserIdIsEmpty()
        {
            //Arrange
            var userId = new UserId(Guid.Empty);
            var planner = new PlannerBuilder().Build();

            //Act & Assert
            Assert.Throws<ArgumentException>(() => planner.Delete(userId));
        }

        [Fact]
        public void Delete_ShouldReturnFailureResult_WhenRequestedByUserIdIsNotOwnerId()
        {
            //Arrange
            var planner = new PlannerBuilder().WithOwnerId(new UserId(Guid.NewGuid())).Build();
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
            var planner = new PlannerBuilder().WithOwnerId(new UserId(Guid.NewGuid())).Build();

            //Act
            var result = planner.Delete(planner.OwnerId);

            //Assert
            Assert.True(result.IsSuccess);
        }
    }
}
