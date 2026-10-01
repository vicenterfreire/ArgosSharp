using ArgosSharp.Api.DependeciesInjection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace ArgosSharp.Api.UnitTests.DependeciesInjection
{
    [TestFixture]
    public class MiddlewareDependenciesTests
    {
        [Test]
        public void AddMiddlewareDependencies_ShouldReturnSameServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddMiddlewareDependencies();

            // Assert
            result.Should().BeSameAs(services);
        }
    }
}

