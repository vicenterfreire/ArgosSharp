using ArgosSharp.Application.Contracts.Jobs;
using ArgosSharp.Application.Interfaces.Repositories;
using ArgosSharp.Application.UseCase.Jobs.CreateJob;
using ArgosSharp.Application.UseCase.Jobs.GetJob;
using ArgosSharp.Domain.Entity;
using ArgosSharp.Domain.Enums;
using ArgosSharp.Domain.Factories.JobFactory;
using ArgosSharp.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace ArgosSharp.Application.UnitTests.UseCase;

public class CreateJobUseCaseTests
{
    private Mock<IJobFactory> _jobFactory = null!;
    private Mock<IJobRepository> _jobRepository = null!;
    private CreateJobUseCase _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _jobFactory = new Mock<IJobFactory>(MockBehavior.Strict);
        _jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
        _sut = new CreateJobUseCase(_jobFactory.Object, _jobRepository.Object);
    }

    [Test]
    public async Task CreateJob_WhenRequestIsValid_ShouldPersistAndReturnCreatedJob()
    {
        // Arrange
        var parameters = new JobParameters("python", ["caraguatatuba"], 1);
        var request = new CreateJobRequest
        {
            Name = "Python jobs",
            JobType = JobType.NewsArticles,
            Parameters = parameters,
            Priority = JobPriority.High,
            Enabled = true
        };
        var expectedJob = new Job(request.Name, request.JobType, request.Parameters, request.Priority, request.Enabled);

        _jobFactory
            .Setup(x => x.Create(request.Name, request.JobType, request.Parameters, request.Priority, request.Enabled))
            .Returns(expectedJob);

        _jobRepository
            .Setup(x => x.AddAsync(expectedJob))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.CreateJob(request);

        // Assert
        result.Should().BeSameAs(expectedJob);
        _jobRepository.Verify(x => x.AddAsync(expectedJob), Times.Once);
    }

    [Test]
    public async Task CreateJob_WhenRepositoryAddFails_ShouldPropagateException()
    {
        // Arrange
        var parameters = new JobParameters("python", ["caraguatatuba"], 1);
        var request = new CreateJobRequest
        {
            Name = "Python jobs",
            JobType = JobType.NewsArticles,
            Parameters = parameters,
            Priority = JobPriority.High,
            Enabled = true
        };
        var job = new Job(request.Name, request.JobType, request.Parameters, request.Priority, request.Enabled);

        _jobFactory
            .Setup(x => x.Create(request.Name, request.JobType, request.Parameters, request.Priority, request.Enabled))
            .Returns(job);

        _jobRepository
            .Setup(x => x.AddAsync(job))
            .ThrowsAsync(new InvalidOperationException("Repository failed"));

        // Act
        Func<Task> act = () => _sut.CreateJob(request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Repository failed");
    }

    [Test]
    public async Task GetJobsAsync_WhenRepositoryReturnsJobs_ShouldReturnThem()
    {
        // Arrange
        var jobs = new List<Job>
        {
            new("Job 1", JobType.NewsArticles, new JobParameters("python", ["caraguatatuba"], 1), JobPriority.Normal, true),
            new("Job 2", JobType.JobPosting, new JobParameters("csharp", ["ubatuba"], 1), JobPriority.High, false)
        };
        var sut = new GetJobUseCase(_jobRepository.Object);

        _jobRepository
            .Setup(x => x.GetAllAsync())
            .ReturnsAsync(jobs);

        // Act
        var result = await sut.GetJobsAsync();

        // Assert
        result.Should().BeEquivalentTo(jobs);
    }
}
