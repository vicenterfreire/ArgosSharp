using ArgosSharp.Application.Services.JobExecutionQueue;
using ArgosSharp.Domain.Entity;
using ArgosSharp.Domain.Enums;
using ArgosSharp.Domain.ValueObjects;
using FluentAssertions;

namespace ArgosSharp.Application.UnitTests.Services;

public class JobExecutionQueueTests
{
    [Test]
    public async Task EnqueueAsync_WhenJobExecutionIsQueued_ShouldReturnItOnDequeue()
    {
        // Arrange
        var queue = new JobExecutionQueue();
        var jobExecution = new JobExecution(1, JobStatusEnum.Created, new JobParameters("python", ["caraguatatuba"], 1));

        // Act
        await queue.EnqueueAsync(jobExecution);
        var result = await queue.DequeueAsync(CancellationToken.None);

        // Assert
        result.Should().BeSameAs(jobExecution);
    }

    [Test]
    public async Task DequeueAsync_WhenCancelled_ShouldThrowOperationCanceledException()
    {
        // Arrange
        var queue = new JobExecutionQueue();
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        Func<Task> act = () => queue.DequeueAsync(cts.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }
}
