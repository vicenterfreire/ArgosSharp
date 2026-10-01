using ArgosSharp.Application.Interfaces.Repositories;
using ArgosSharp.Application.Services.ArtifactsService;
using ArgosSharp.Application.Services.ExporterService;
using ArgosSharp.Application.Services.JobProcessor;
using ArgosSharp.Application.UseCase.Scraper;
using ArgosSharp.Domain.Entity;
using ArgosSharp.Domain.Enums;
using ArgosSharp.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace ArgosSharp.Application.UnitTests.Services;

public class JobProcessorServiceTests
{
    private Mock<IJobExecutionRepository> _jobExecutionRepository = null!;
    private Mock<IScraperContext> _scraperContext = null!;
    private Mock<IJobRepository> _jobRepository = null!;
    private Mock<IExportService> _exportService = null!;
    private Mock<IArtifactsService> _artifactsService = null!;

    [SetUp]
    public void SetUp()
    {
        _jobExecutionRepository = new Mock<IJobExecutionRepository>(MockBehavior.Strict);
        _scraperContext = new Mock<IScraperContext>(MockBehavior.Strict);
        _jobRepository = new Mock<IJobRepository>(MockBehavior.Strict);
        _exportService = new Mock<IExportService>(MockBehavior.Strict);
        _artifactsService = new Mock<IArtifactsService>(MockBehavior.Strict);
    }

    [Test]
    public async Task ProcessJobAsync_WhenNewsJobIsProcessed_ShouldCompleteExecutionAndPersistArtifact()
    {
        // Arrange
        var job = new Job("News job", JobType.NewsArticles, new JobParameters("python", ["caraguatatuba"], 1), JobPriority.Normal, true);
        var jobExecution = new JobExecution(job.Id, JobStatusEnum.Created, job.Parameters);
        var newsArticles = new List<NewsArticle>
        {
            new("Title 1", DateTime.UtcNow, 2025, "https://example.com/1", "summary", "caraguatatuba"),
            new("Title 2", DateTime.UtcNow, 2025, "https://example.com/2", "summary", "caraguatatuba")
        };
        var export = new ResultExport(new MemoryStream(new byte[] { 1, 2, 3 }), "results.csv", "text/csv", ".csv");

        _jobExecutionRepository.Setup(x => x.UpdateAsync(jobExecution)).Returns(Task.CompletedTask);
        _jobRepository.Setup(x => x.GetAsync(job.Id)).ReturnsAsync(job);
        _scraperContext.Setup(x => x.GetNewsBySourceAsync(job.Parameters)).ReturnsAsync(newsArticles);
        _exportService.Setup(x => x.ExportAsync(It.IsAny<JobExecutionResult>(), ExportFormat.CSV)).ReturnsAsync(export);
        _artifactsService.Setup(x => x.CreateAsync(jobExecution.Id, export.Content, export.FileName, export.ContentType, CancellationToken.None)).ReturnsAsync(new Artifact(jobExecution.Id, export.FileName, export.ContentType, export.Content.Length, "executions/1/results.csv"));

        var sut = new JobProcessorService(_jobExecutionRepository.Object, _scraperContext.Object, _jobRepository.Object, _exportService.Object, _artifactsService.Object);

        // Act
        await sut.ProcessJobAsync(jobExecution);

        // Assert
        jobExecution.JobStatus.Should().Be(JobStatusEnum.Completed);
        _jobExecutionRepository.Verify(x => x.UpdateAsync(jobExecution), Times.Exactly(2));
        _exportService.Verify(x => x.ExportAsync(It.Is<JobExecutionResult>(r => r.JobType == typeof(NewsArticle)), ExportFormat.CSV), Times.Once);
        _artifactsService.Verify(x => x.CreateAsync(jobExecution.Id, export.Content, export.FileName, export.ContentType, CancellationToken.None), Times.Once);
    }

    [Test]
    public async Task ProcessJobAsync_WhenJobDoesNotExist_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var job = new Job("News job", JobType.NewsArticles, new JobParameters("python", ["caraguatatuba"], 1), JobPriority.Normal, true);
        var jobExecution = new JobExecution(42, JobStatusEnum.Created, job.Parameters);

        _jobExecutionRepository.Setup(x => x.UpdateAsync(jobExecution)).Returns(Task.CompletedTask);
        _jobRepository.Setup(x => x.GetAsync(jobExecution.JobId)).ReturnsAsync((Job?)null);

        var sut = new JobProcessorService(_jobExecutionRepository.Object, _scraperContext.Object, _jobRepository.Object, _exportService.Object, _artifactsService.Object);

        // Act
        Func<Task> act = () => sut.ProcessJobAsync(jobExecution);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>().WithMessage("Job is null");
    }
}
