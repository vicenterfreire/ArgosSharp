using ArgosSharp.Application.Interfaces.Fetcher;
using ArgosSharp.Application.Interfaces.Parser;
using ArgosSharp.Application.Interfaces.Scrapers;
using ArgosSharp.Domain.ValueObjects;
using ArgosSharp.Infrastructure.Mapper;
using ArgosSharp.Infrastructure.Extractor;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ArgosSharp.Infrastructure.Scrapers.NewsArticles
{
    public class UbatubaScraper(ILogger<UbatubaScraper> logger, IHttpFetcher fetcher, IHtmlParser parser) : INewsArticlesScrapers
    {
        public string Name { get; set; } = "ubatuba";

        private const string BaseUrl = "https://www.ubatuba.sp.gov.br/?s=";
        private const string PageBaseUrl = "https://www.ubatuba.sp.gov.br/page/{0}/?s=";

        private static readonly ScraperSelectors Selectors = new()
        {
            Pagination = "ul[class*='pagenavi'] > li",
            News = "ul[class*='blog-items'] div[class*='search-item-content']",
            Title = "h3 > a::text()",
            Link = "h3 > a::attr(href)",
            Abstract = "div[class*='excerpt']::text()",
            Date = "time::text()"
        };

        public async Task<List<NewsArticle>> ProcessScraperAsync(string searchTerm, int depth)
        {
            var news = new List<string>();
            var termParsed = WebUtility.UrlEncode(searchTerm);

            logger.LogInformation("Starting scraping for term {SearchTerm} in Ubatuba", searchTerm);

            var firstPageHtml = await fetcher.GetStringAsync(BaseUrl + termParsed);
            var maxPage = GetPaginationIfExists(firstPageHtml);
            var limit = Math.Min(maxPage, depth);

            news.AddRange(GetNewsIfExists(firstPageHtml));
            news.AddRange(await GetNewsFromPagination(termParsed, limit));

            logger.LogInformation("Total news collected: {Total}", news.Count);

            return [..news.Select(x => NewsMapper.Map(x, parser, Selectors, Name))];
        }

        private int GetPaginationIfExists(string html)
        {
            var pagination = parser.QueryTexts(html, Selectors.Pagination).ToList();

            var maxPage = PaginationExtractor.GetMaxPage(pagination);

            logger.LogInformation("Detected {PageQuantity} pages from pagination", maxPage);

            return maxPage;
        }

        private List<string> GetNewsIfExists(string document)
        {
            var news = parser.QueryTexts(document, Selectors.News).ToList();
            logger.LogInformation("News found on page: {NewsCount}", news.Count);
            return news;
        }

        private async Task<List<string>> GetNewsFromPagination(string termParsed, int limit)
        {
            List<string> newsRaw = [];
            for (var i = 2; i <= limit; i++)
            {
                var html = await fetcher.GetStringAsync(string.Format(PageBaseUrl, i) + termParsed);
                var news = GetNewsIfExists(html);
                if (news.Count > 0)
                    newsRaw.AddRange(news);
            }
            return newsRaw;
        }
    }
}
