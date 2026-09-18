using ArgosSharp.Domain.Entity;
using ArgosSharp.Domain.Enums;
using ArgosSharp.Domain.ValueObjects;
using FluentAssertions;

namespace ArgosSharp.Domain.UnitTests.Entity;

public sealed class JobExecutionTests
{
    [Test]
    public void Constructor_WithValidData_ShouldCreateJobExecution()
    {
        // Arrange
        const int jobId = 1;
        const JobStatusEnum status = JobStatusEnum.Created;
        var parameters = CreateValidParameters();

        // Act
        var execution = new JobExecution(jobId, status, parameters);

        // Assert
        execution.JobId.Should().Be(jobId);
        execution.JobStatus.Should().Be(status);
        execution.Parameters.Should().BeSameAs(parameters);
        execution.QueuedAt.Should().BeNull();
        execution.StartedAt.Should().BeNull();
        execution.FinishedAt.Should().BeNull();
        execution.Error.Should().BeNull();
    }

    [Test]
    public void Constructor_WithInvalidStatus_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        const JobStatusEnum invalidStatus = (JobStatusEnum)999;

        // Act
        var action = () => new JobExecution(1, invalidStatus, CreateValidParameters());

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("jobStatus");
    }

    [Test]
    public void Constructor_WithNullParameters_ShouldThrowArgumentNullException()
    {
        // Act
        var action = () => new JobExecution(1, JobStatusEnum.Created, null!);

        // Assert
        action.Should().Throw<ArgumentNullException>().WithParameterName("parameters");
    }

    [Test]
    public void Enqueued_ShouldSetStatusAndQueuedAt()
    {
        // Arrange
        var execution = CreateJobExecution();
        var before = DateTime.UtcNow;

        // Act
        execution.Enqueued();
        var after = DateTime.UtcNow;

        // Assert
        execution.JobStatus.Should().Be(JobStatusEnum.Enqueued);
        execution.QueuedAt.Should().NotBeNull();
        execution.QueuedAt.Should().BeOnOrAfter(before);
        execution.QueuedAt.Should().BeOnOrBefore(after);
    }

    [Test]
    public void StartProcessing_ShouldSetStatusAndStartedAt()
    {
        // Arrange
        var execution = CreateJobExecution();
        var before = DateTime.UtcNow;

        // Act
        execution.StartProcessing();
        var after = DateTime.UtcNow;

        // Assert
        execution.JobStatus.Should().Be(JobStatusEnum.Processing);
        execution.StartedAt.Should().NotBeNull();
        execution.StartedAt.Should().BeOnOrAfter(before);
        execution.StartedAt.Should().BeOnOrBefore(after);
    }

    [Test]
    public void CompleteProcessing_ShouldSetStatusAndFinishedAt()
    {
        // Arrange
        var execution = CreateJobExecution();
        var before = DateTime.UtcNow;

        // Act
        execution.CompleteProcessing();
        var after = DateTime.UtcNow;

        // Assert
        execution.JobStatus.Should().Be(JobStatusEnum.Completed);
        execution.FinishedAt.Should().NotBeNull();
        execution.FinishedAt.Should().BeOnOrAfter(before);
        execution.FinishedAt.Should().BeOnOrBefore(after);
    }

    [Test]
    public void FailProcessing_ShouldSetStatusFinishedAtAndError()
    {
        // Arrange
        var execution = CreateJobExecution();
        const string error = "Scraper failed.";
        var before = DateTime.UtcNow;

        // Act
        execution.FailProcessing(error);
        var after = DateTime.UtcNow;

        // Assert
        execution.JobStatus.Should().Be(JobStatusEnum.Failed);
        execution.FinishedAt.Should().NotBeNull();
        execution.FinishedAt.Should().BeOnOrAfter(before);
        execution.FinishedAt.Should().BeOnOrBefore(after);
        execution.Error.Should().Be(error);
    }

    private static JobExecution CreateJobExecution()
    {
        return new JobExecution(1, JobStatusEnum.Created, CreateValidParameters());
    }

    private static JobParameters CreateValidParameters()
    {
        return new JobParameters("technology", ["https://example.com"], 1);
    }
}
