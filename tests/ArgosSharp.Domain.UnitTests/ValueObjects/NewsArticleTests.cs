using ArgosSharp.Domain.ValueObjects;
using FluentAssertions;

namespace ArgosSharp.Domain.UnitTests.ValueObjects
{
    [TestFixture]
    public class NewsArticleTests
    {
        [Test]
        public void DefaultPropertyValues_ShouldBeEmptyStrings()
        {
            // Act
            var article = new NewsArticle("Title", null, 2026, "Link", null, "Source");

            // Assert
            article.Title.Should().Be("Title");
            article.Link.Should().Be("Link");
            article.Abstract.Should().Be(string.Empty);
            article.Source.Should().Be("Source");
        }

        [Test]
        public void Constructor_WithValidAbstract_ShouldSetAbstractProperty()
        {
            // Act
            var article = new NewsArticle("Title", DateTime.UtcNow, 2026, "Link", "Summary text", "Source");

            // Assert
            article.Abstract.Should().Be("Summary text");
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        public void Constructor_WithNullOrWhitespaceAbstract_ShouldKeepAbstractAsEmptyString(string? @abstract)
        {
            // Act
            var article = new NewsArticle("Title", DateTime.UtcNow, 2026, "Link", @abstract, "Source");

            // Assert
            article.Abstract.Should().Be(string.Empty);
        }
    }
}

