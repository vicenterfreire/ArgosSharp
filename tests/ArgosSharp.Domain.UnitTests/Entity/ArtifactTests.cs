using ArgosSharp.Domain.Entity;
using FluentAssertions;

namespace ArgosSharp.Domain.UnitTests.Entity;

public sealed class ArtifactTests
{
    [Test]
    public void Constructor_WithValidData_ShouldCreateArtifact()
    {
        // Arrange
        const int jobExecutionId = 1;
        const string fileName = "result.csv";
        const string contentType = "text/csv";
        const long size = 1;
        const string storagePath = "executions/1/result.csv";

        // Act
        var artifact = new Artifact(jobExecutionId, fileName, contentType, size, storagePath);

        // Assert
        artifact.JobExecutionId.Should().Be(jobExecutionId);
        artifact.FileName.Should().Be(fileName);
        artifact.ContentType.Should().Be(contentType);
        artifact.Size.Should().Be(size);
        artifact.StoragePath.Should().Be(storagePath);
    }

    [Test]
    public void Constructor_WithNullFileName_ShouldNotThrow()
    {
        // Act
        var action = () => new Artifact(1, null!, "text/csv", 1, "executions/1/result.csv");

        // Assert
        action.Should().NotThrow();

        var artifact = new Artifact(1, null!, "text/csv", 1, "executions/1/result.csv");
        artifact.FileName.Should().BeNull();
    }

    [Test]
    public void Constructor_WithEmptyFileName_ShouldNotThrow()
    {
        // Act
        var action = () => new Artifact(1, string.Empty, "text/csv", 1, "executions/1/result.csv");

        // Assert
        action.Should().NotThrow();

        var artifact = new Artifact(1, string.Empty, "text/csv", 1, "executions/1/result.csv");
        artifact.FileName.Should().BeEmpty();
    }

    [Test]
    public void Constructor_WithNullContentType_ShouldNotThrow()
    {
        // Act
        var action = () => new Artifact(1, "result.csv", null!, 1, "executions/1/result.csv");

        // Assert
        action.Should().NotThrow();

        var artifact = new Artifact(1, "result.csv", null!, 1, "executions/1/result.csv");
        artifact.ContentType.Should().BeNull();
    }

    [Test]
    public void Constructor_WithEmptyContentType_ShouldNotThrow()
    {
        // Act
        var action = () => new Artifact(1, "result.csv", string.Empty, 1, "executions/1/result.csv");

        // Assert
        action.Should().NotThrow();

        var artifact = new Artifact(1, "result.csv", string.Empty, 1, "executions/1/result.csv");
        artifact.ContentType.Should().BeEmpty();
    }

    [Test]
    public void Constructor_WithNullStoragePath_ShouldNotThrow()
    {
        // Act
        var action = () => new Artifact(1, "result.csv", "text/csv", 1, null!);

        // Assert
        action.Should().NotThrow();

        var artifact = new Artifact(1, "result.csv", "text/csv", 1, null!);
        artifact.StoragePath.Should().BeNull();
    }

    [Test]
    public void Constructor_WithEmptyStoragePath_ShouldNotThrow()
    {
        // Act
        var action = () => new Artifact(1, "result.csv", "text/csv", 1, string.Empty);

        // Assert
        action.Should().NotThrow();

        var artifact = new Artifact(1, "result.csv", "text/csv", 1, string.Empty);
        artifact.StoragePath.Should().BeEmpty();
    }

    [TestCase(2)]
    [TestCase(1024)]
    public void Constructor_WithSizeGreaterThanOne_ShouldThrowArgumentOutOfRangeException(long size)
    {
        // Act
        var action = () => new Artifact(1, "result.csv", "text/csv", size, "executions/1/result.csv");

        // Assert
        action.Should().Throw<ArgumentOutOfRangeException>().WithParameterName("size");
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(1)]
    public void Constructor_WithSizeLessThanOrEqualToOne_ShouldCreateArtifact(long size)
    {
        // Act
        var artifact = new Artifact(1, "result.csv", "text/csv", size, "executions/1/result.csv");

        // Assert
        artifact.Size.Should().Be(size);
    }
}
