using ArgosSharp.Domain.Entity;
using ArgosSharp.Domain.Enums;
using ArgosSharp.Domain.ValueObjects;
using FluentAssertions;

namespace ArgosSharp.Domain.UnitTests.Entity;

public sealed class JobTests
{
    [Test]
    public void Constructor_WithValidData_ShouldCreateJob()
    {
        // Arrange
        const string name = "News Scraper";
        const JobType jobType = JobType.NewsArticles;
        const JobPriority priority = JobPriority.Normal;
        const bool enabled = true;
        var parameters = CreateValidParameters();

        // Act
        var job = new Job(name, jobType, parameters, priority, enabled);

        // Assert
        job.Name.Should().Be(name);
        job.JobType.Should().Be(jobType);
        job.Parameters.Should().BeSameAs(parameters);
        job.Priority.Should().Be(priority);
        job.Enabled.Should().Be(enabled);
    }

    [Test]
    public void Constructor_WithNullName_ShouldThrowArgumentException()
    {
        // Arrange
        var parameters = CreateValidParameters();

        // Act
        var action = () => new Job(null!, JobType.NewsArticles, parameters, JobPriority.Normal, true);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_WithEmptyName_ShouldThrowArgumentException()
    {
        // Arrange
        var parameters = CreateValidParameters();

        // Act
        var action = () => new Job(string.Empty, JobType.NewsArticles, parameters, JobPriority.Normal, true);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Test]
    public void Constructor_WithInvalidJobType_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var invalidJobType = (JobType)999;

        // Act
        var action = () => new Job("News Scraper", invalidJobType, CreateValidParameters(), JobPriority.Normal, true);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("jobType");
    }

    [Test]
    public void Constructor_WithNullParameters_ShouldThrowArgumentNullException()
    {
        // Act
        var action = () => new Job("News Scraper", JobType.NewsArticles, null!, JobPriority.Normal, true);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_WithInvalidPriority_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        var invalidPriority = (JobPriority)999;

        // Act
        var action = () => new Job("News Scraper", JobType.NewsArticles, CreateValidParameters(), invalidPriority, true);

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("priority");
    }

    [Test]
    public void Constructor_WithDisabledState_ShouldCreateDisabledJob()
    {
        // Act
        var job = new Job("News Scraper", JobType.NewsArticles, CreateValidParameters(), JobPriority.Normal, false);

        // Assert
        job.Enabled.Should().BeFalse();
    }

    private static JobParameters CreateValidParameters()
    {
        return new JobParameters("technology", ["https://example.com"], 1);
    }
}
