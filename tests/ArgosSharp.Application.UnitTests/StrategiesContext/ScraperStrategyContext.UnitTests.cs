using ArgosSharp.Application.Interfaces.Scrapers;
using ArgosSharp.Application.UseCase.Scraper;
using ArgosSharp.Domain.Enums;
using ArgosSharp.Domain.ValueObjects;
using FluentAssertions;
using Moq;

namespace ArgosSharp.Application.UnitTests.StrategiesContext;

public class ScraperContextTests
{
    [Test]
    public async Task GetNewsBySourceAsync_WhenSourceExists_ShouldReturnNewsFromThatSource()
    {
        // Arrange
        var newsResults = new List<NewsArticle>
        {
            new("Title 1", DateTime.UtcNow, 2025, "https://example.com/1", "summary", "caraguatatuba"),
            new("Title 2", DateTime.UtcNow, 2025, "https://example.com/2", "summary", "caraguatatuba")
        };
        var newsScraper = new Mock<INewsArticlesScrapers>(MockBehavior.Strict);
        newsScraper.SetupGet(x => x.Name).Returns("caraguatatuba");
        newsScraper.Setup(x => x.ProcessScraperAsync("python", 1)).ReturnsAsync(newsResults);

        var jobPostingScraper = new Mock<IJobPostingsScrapers>(MockBehavior.Strict);
        jobPostingScraper.SetupGet(x => x.Name).Returns("other-source");
        var sut = new ScraperContext([newsScraper.Object], [jobPostingScraper.Object]);
        var parameters = new JobParameters("python", ["caraguatatuba"], 1);

        // Act
        var result = await sut.GetNewsBySourceAsync(parameters);

        // Assert
        result.Should().BeEquivalentTo(newsResults);
    }

    [Test]
    public async Task GetJobsBySourceAsync_WhenSourceExists_ShouldReturnJobsFromThatSource()
    {
        // Arrange
        var jobResults = new List<JobPosting>
        {
            new("Developer", "https://example.com/job/1", "C# developer", "ubatuba"),
            new("Analyst", "https://example.com/job/2", "Data analyst", "ubatuba")
        };
        var newsScraper = new Mock<INewsArticlesScrapers>(MockBehavior.Strict);
        newsScraper.SetupGet(x => x.Name).Returns("other-news-source");
        var jobScraper = new Mock<IJobPostingsScrapers>(MockBehavior.Strict);
        jobScraper.SetupGet(x => x.Name).Returns("ubatuba");
        jobScraper.Setup(x => x.ProcessScraperAsync("csharp", 1)).ReturnsAsync(jobResults);

        var sut = new ScraperContext([newsScraper.Object], [jobScraper.Object]);
        var parameters = new JobParameters("csharp", ["ubatuba"], 1);

        // Act
        var result = await sut.GetJobsBySourceAsync(parameters);

        // Assert
        result.Should().BeEquivalentTo(jobResults);
    }

    [Test]
    public async Task GetNewsBySourceAsync_WhenSourceDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var newsScraper = new Mock<INewsArticlesScrapers>(MockBehavior.Strict);
        newsScraper.SetupGet(x => x.Name).Returns("caraguatatuba");

        var sut = new ScraperContext([newsScraper.Object], []);
        var parameters = new JobParameters("python", ["unknown-source"], 1);

        // Act
        Func<Task> act = () => sut.GetNewsBySourceAsync(parameters);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}
