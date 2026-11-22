using TaskManagement.Domain.Entities;

namespace TaskManagement.Tests;

public class TaskItemTest
{
    [Fact]
    public void Reopen_WhenTaskIsCompeleted_MarksTaskAsIncomplete()
    {
        //Arrange
        var task = TaskItem.Create("Read Chapter one.", null, 1).Value;
        task.Complete();

        //Act
        task.Reopen();

        //Assert
        Assert.False(task.IsCompleted);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    public void ChangePriority_whenPriorityIsTooHigh_RejectsChangesAndKeepTheOriginal(int invalidPriority)
    {
        var task = TaskItem.Create("Exam Tommorrow.", null, 1, 2).Value;

        var result = task.ChangePriority(4);

        Assert.True(result.IsFailure);
        Assert.Equal(2, task.Priority);
    }

    [Fact]
    public void ChangePriority_WhenPriorityIsThree_UpdatesPriority()
    {
        var task = TaskItem.Create("Exam Tommorrow.", null, 1, 2).Value;
        var result = task.ChangePriority(3);
        Assert.True(result.IsSuccess);
        Assert.Equal(3, task.Priority);
    }
}
