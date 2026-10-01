using ArgosSharp.Application.Interfaces.Parser;
using ArgosSharp.Domain.ValueObjects;
using ArgosSharp.Infrastructure.Mapper;
using FluentAssertions;
using Moq;

namespace ArgosSharp.Infrastructure.UnitTests.Mapper
{
    [TestFixture]
    public class NewsMapperTests
    {
        [Test]
        public void Map_WithValidDateFormat_ShouldParseDateAndYearCorrectly()
        {
            // Arrange
            var parserMock = new Mock<IHtmlParser>();
            var selectors = new ScraperSelectors
            {
                Date = "selectors.date",
                Title = "selectors.title",
                Link = "selectors.link",
                Abstract = "selectors.abstract"
            };

            parserMock.Setup(p => p.QueryText("html", selectors.Date)).Returns("25/12/2026");
            parserMock.Setup(p => p.QueryText("html", selectors.Title)).Returns("Title Test");
            parserMock.Setup(p => p.QueryText("html", selectors.Link)).Returns("https://example.com");
            parserMock.Setup(p => p.QueryText("html", selectors.Abstract)).Returns("Summary");

            // Act
            var article = NewsMapper.Map("html", parserMock.Object, selectors, "caraguatatuba");

            // Assert
            article.DateTime.Should().Be(new DateTime(2026, 12, 25));
            article.Year.Should().Be(2026);
            article.Title.Should().Be("Title Test");
            article.Link.Should().Be("https://example.com");
            article.Abstract.Should().Be("Summary");
            article.Source.Should().Be("caraguatatuba");
        }

        [Test]
        public void Map_WhenAbstractIsNull_ShouldFallbackToEmptyString()
        {
            // Arrange
            var parserMock = new Mock<IHtmlParser>();
            var selectors = new ScraperSelectors
            {
                Date = "selectors.date",
                Title = "selectors.title",
                Link = "selectors.link",
                Abstract = "selectors.abstract"
            };

            parserMock.Setup(p => p.QueryText("html", selectors.Date)).Returns("01/01/2026");
            parserMock.Setup(p => p.QueryText("html", selectors.Title)).Returns("Title");
            parserMock.Setup(p => p.QueryText("html", selectors.Link)).Returns("Link");
            parserMock.Setup(p => p.QueryText("html", selectors.Abstract)).Returns((string?)null);

            // Act
            var article = NewsMapper.Map("html", parserMock.Object, selectors, "source");

            // Assert
            article.Abstract.Should().Be(string.Empty);
        }

        [Test]
        public void Map_WhenTitleAndLinkAreNull_ShouldFallbackToDefaultStrings()
        {
            // Arrange
            var parserMock = new Mock<IHtmlParser>();
            var selectors = new ScraperSelectors();

            // Act
            var article = NewsMapper.Map("html", parserMock.Object, selectors, "source");

            // Assert
            article.Title.Should().Be("No title");
            article.Link.Should().Be("No link");
            article.Abstract.Should().Be(string.Empty);
        }
    }
}

