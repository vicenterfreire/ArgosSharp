using ArgosSharp.Api.Controllers;
using ArgosSharp.Application.Contracts.Jobs;
using ArgosSharp.Application.UseCase.Jobs.CreateJob;
using ArgosSharp.Application.UseCase.Jobs.GetJob;
using ArgosSharp.Domain.Entity;
using ArgosSharp.Domain.Enums;
using ArgosSharp.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ArgosSharp.Api.UnitTests.Controllers
{
    [TestFixture]
    public class JobsControllerTests
    {
        private Mock<ICreateJobUseCase> _createJobUseCaseMock;
        private Mock<IGetJobUseCase> _getJobUseCaseMock;
        private JobsController _jobsController;

        [SetUp]
        public void Setup()
        {
            _createJobUseCaseMock = new Mock<ICreateJobUseCase>();
            _getJobUseCaseMock = new Mock<IGetJobUseCase>();

            _jobsController = new JobsController(
                _createJobUseCaseMock.Object,
                _getJobUseCaseMock.Object);
        }

        [Test]
        public async Task GetJobsAsync_WhenJobsExist_ShouldReturnListOfJobs()
        {
            // Arrange
            var expectedJobs = new List<Job>
            {
                new("Job 1", JobType.NewsArticles, new JobParameters("python", ["https://example.com"], 1), JobPriority.High, true),
                new("Job 2", JobType.JobPosting, new JobParameters("c#", ["https://example.org"], 2), JobPriority.Normal, true)
            };

            _getJobUseCaseMock
                .Setup(x => x.GetJobsAsync())
                .ReturnsAsync(expectedJobs);

            // Act
            var result = await _jobsController.GetJobsAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedJobs);
            _getJobUseCaseMock.Verify(x => x.GetJobsAsync(), Times.Once);
        }

        [Test]
        public async Task GetJobsAsync_WhenNoJobsExist_ShouldReturnEmptyList()
        {
            // Arrange
            _getJobUseCaseMock
                .Setup(x => x.GetJobsAsync())
                .ReturnsAsync(new List<Job>());

            // Act
            var result = await _jobsController.GetJobsAsync();

            // Assert
            result.Should().BeEmpty();
            _getJobUseCaseMock.Verify(x => x.GetJobsAsync(), Times.Once);
        }

        [Test]
        public async Task CreateJobAsync_WhenRequestIsValid_ShouldReturnOkWithJob()
        {
            // Arrange
            var request = new CreateJobRequest
            {
                Name = "Test Job",
                JobType = JobType.NewsArticles,
                Parameters = new JobParameters("python", ["https://example.com"], 1),
                Priority = JobPriority.High,
                Enabled = true
            };

            var expectedJob = new Job("Test Job", JobType.NewsArticles, request.Parameters, JobPriority.High, true);

            _createJobUseCaseMock
                .Setup(x => x.CreateJob(request))
                .ReturnsAsync(expectedJob);

            // Act
            var result = await _jobsController.CreateJobAsync(request);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var job = okResult.Value.Should().BeOfType<Job>().Subject;
            job.Should().BeEquivalentTo(expectedJob);
            _createJobUseCaseMock.Verify(x => x.CreateJob(request), Times.Once);
        }
    }
}
