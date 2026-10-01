using ArgosSharp.Application.Services.JobExecutionQueue;
using ArgosSharp.Application.Services.JobProcessor;
using ArgosSharp.Application.Services.JobWorker;
using ArgosSharp.Domain.Entity;
using ArgosSharp.Domain.Enums;
using ArgosSharp.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ArgosSharp.Application.UnitTests.Services;

public class JobWorkerTests
{
    [Test]
    public async Task ExecuteAsync_WhenQueueContainsJob_ShouldInvokeProcessorOnce()
    {
        // Arrange
        var jobExecution = new JobExecution(5, JobStatusEnum.Created, new JobParameters("python", ["caraguatatuba"], 1));
        var queue = new Mock<IJobExecutionQueue>(MockBehavior.Strict);
        var processor = new Mock<IJobProcessorService>(MockBehavior.Strict);
        var scope = new Mock<IServiceScope>(MockBehavior.Loose);
        var serviceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);
        var scopeFactory = new Mock<IServiceScopeFactory>(MockBehavior.Strict);
        var processed = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        queue.SetupSequence(x => x.DequeueAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(jobExecution)
            .Returns(new TaskCompletionSource<JobExecution>().Task);
        processor.Setup(x => x.ProcessJobAsync(jobExecution)).Returns(Task.CompletedTask).Callback(() => processed.SetResult());
        serviceProvider.Setup(x => x.GetService(typeof(IJobProcessorService))).Returns(processor.Object);
        scope.SetupGet(x => x.ServiceProvider).Returns(serviceProvider.Object);
        scopeFactory.Setup(x => x.CreateScope()).Returns(scope.Object);
        scope.Setup(x => x.Dispose());

        var worker = new JobWorker(queue.Object, scopeFactory.Object);

        // Act
        await worker.StartAsync(CancellationToken.None);
        await processed.Task.WaitAsync(TimeSpan.FromSeconds(5));
        await worker.StopAsync(CancellationToken.None);

        // Assert
        processor.Verify(x => x.ProcessJobAsync(jobExecution), Times.Once);
    }

    [Test]
    public async Task ExecuteAsync_WhenTokenIsAlreadyCancelled_ShouldNotDequeueOrProcessJob()
    {
        // Arrange
        var queue = new Mock<IJobExecutionQueue>(MockBehavior.Strict);
        var scopeFactory = new Mock<IServiceScopeFactory>(MockBehavior.Strict);
        var worker = new JobWorker(queue.Object, scopeFactory.Object);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        await worker.StartAsync(cts.Token);

        // Assert
        queue.Verify(x => x.DequeueAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
