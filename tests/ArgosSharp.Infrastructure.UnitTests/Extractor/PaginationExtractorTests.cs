using ArgosSharp.Infrastructure.Extractor;
using FluentAssertions;

namespace ArgosSharp.Infrastructure.UnitTests.Extractor
{
    [TestFixture]
    public class PaginationExtractorTests
    {
        [Test]
        public void GetMaxPage_WhenNull_ShouldReturn1()
        {
            // Act
            var maxPage = PaginationExtractor.GetMaxPage(null);

            // Assert
            maxPage.Should().Be(1);
        }

        [Test]
        public void GetMaxPage_WhenEmpty_ShouldReturn1()
        {
            // Act
            var maxPage = PaginationExtractor.GetMaxPage(Enumerable.Empty<string>());

            // Assert
            maxPage.Should().Be(1);
        }

        [Test]
        public void GetMaxPage_WhenSingleItem_ShouldReturn1()
        {
            // Act
            var maxPage = PaginationExtractor.GetMaxPage(new[] { "5" });

            // Assert
            maxPage.Should().Be(1);
        }

        [Test]
        public void GetMaxPage_WhenMultipleItems_ShouldReturnMaxPageNumber()
        {
            // Act
            var maxPage = PaginationExtractor.GetMaxPage(new[] { "1", "3", "2" });

            // Assert
            maxPage.Should().Be(3);
        }

        [Test]
        public void GetMaxPage_WhenNonNumericItemsIncluded_ShouldFilterAndReturnMax()
        {
            // Act
            var maxPage = PaginationExtractor.GetMaxPage(new[] { "invalid", "10", "4" });

            // Assert
            maxPage.Should().Be(10);
        }
    }
}

