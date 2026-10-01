using ArgosSharp.Api.Middlewares;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace ArgosSharp.Api.UnitTests.Middlewares
{
    [TestFixture]
    public class MainMiddlewareTests
    {
        private Mock<ILogger<MainMiddleware>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<MainMiddleware>>();
        }

        [Test]
        public async Task InvokeAsync_WhenNextSucceeds_ShouldLogInformationAndInvokeNext()
        {
            // Arrange
            var nextCalled = false;
            RequestDelegate next = (context) =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new MainMiddleware(next, _loggerMock.Object);
            var httpContext = new DefaultHttpContext();

            // Act
            await middleware.InvokeAsync(httpContext);

            // Assert
            nextCalled.Should().BeTrue();
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Executing Rote")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Test]
        public async Task InvokeAsync_WhenNextThrowsException_ShouldLogErrorAndHandleException()
        {
            // Arrange
            RequestDelegate next = (context) => throw new InvalidOperationException("Test route error");

            var middleware = new MainMiddleware(next, _loggerMock.Object);
            var httpContext = new DefaultHttpContext();

            // Act
            var act = async () => await middleware.InvokeAsync(httpContext);

            // Assert
            await act.Should().NotThrowAsync();
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("An error ocurred executing rote")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}

