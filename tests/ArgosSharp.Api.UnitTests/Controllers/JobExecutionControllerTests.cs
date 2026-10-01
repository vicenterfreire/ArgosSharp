using ArgosSharp.Api.Controllers;
using ArgosSharp.Application.Contracts.JobExecutions;
using ArgosSharp.Application.Interfaces.Repositories;
using ArgosSharp.Application.UseCase.JobsExecution.CreateJobExecution;
using ArgosSharp.Domain.Entity;
using ArgosSharp.Domain.Enums;
using ArgosSharp.Domain.ValueObjects;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ArgosSharp.Api.UnitTests.Controllers
{
    [TestFixture]
    public class JobExecutionControllerTests
    {
        private Mock<ICreateJobExecutionUseCase> _createJobExecutionUseCaseMock;
        private Mock<IJobExecutionRepository> _jobExecutionRepositoryMock;
        private JobExecutionController _controller;

        [SetUp]
        public void Setup()
        {
            _createJobExecutionUseCaseMock = new Mock<ICreateJobExecutionUseCase>();
            _jobExecutionRepositoryMock = new Mock<IJobExecutionRepository>();

            _controller = new JobExecutionController(
                _createJobExecutionUseCaseMock.Object,
                _jobExecutionRepositoryMock.Object);
        }

        [Test]
        public async Task GetJobExecutionsAsync_WhenExecutionsExist_ShouldReturnListOfJobExecutions()
        {
            // Arrange
            var expectedExecutions = new List<JobExecution>
            {
                new(1, JobStatusEnum.Created, new JobParameters("python", ["https://example.com"], 1)),
                new(2, JobStatusEnum.Completed, new JobParameters("c#", ["https://example.org"], 2))
            };

            _jobExecutionRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(expectedExecutions);

            // Act
            var result = await _controller.GetJobExecutionsAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedExecutions);
            _jobExecutionRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task GetJobExecutionsAsync_WhenNoExecutionsExist_ShouldReturnEmptyList()
        {
            // Arrange
            _jobExecutionRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(new List<JobExecution>());

            // Act
            var result = await _controller.GetJobExecutionsAsync();

            // Assert
            result.Should().BeEmpty();
            _jobExecutionRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);
        }

        [Test]
        public async Task CreateJobExecution_WhenRequestIsValid_ShouldReturnOkWithJobExecution()
        {
            // Arrange
            var request = new CreateJobExecutionRequest
            {
                JobId = 1,
                Parameters = new JobParameters("python", ["https://example.com"], 1)
            };

            var expectedExecution = new JobExecution(1, JobStatusEnum.Created, request.Parameters);

            _createJobExecutionUseCaseMock
                .Setup(x => x.CreateJobExecution(request))
                .ReturnsAsync(expectedExecution);

            // Act
            var result = await _controller.CreateJobExecution(request);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var execution = okResult.Value.Should().BeOfType<JobExecution>().Subject;
            execution.Should().BeEquivalentTo(expectedExecution);
            _createJobExecutionUseCaseMock.Verify(x => x.CreateJobExecution(request), Times.Once);
        }
    }
}
