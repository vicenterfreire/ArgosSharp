using ArgosSharp.Api.DependeciesInjection;
using ArgosSharp.Api.Validators;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace ArgosSharp.Api.UnitTests.DependeciesInjection
{
    [TestFixture]
    public class ValidatorsDependenciesTests
    {
        [Test]
        public void AddValidators_ShouldRegisterCreateJobValidatorInServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.AddValidators();

            // Assert
            result.Should().BeSameAs(services);
            services.Should().Contain(descriptor => descriptor.ImplementationType == typeof(CreateJobValidator) || descriptor.ServiceType.Name.Contains("CreateJobValidator"));
        }
    }
}
