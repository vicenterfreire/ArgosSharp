using ArgosSharp.Application.Contracts.Jobs;
using FluentAssertions;

namespace ArgosSharp.Application.UnitTests.Contracts
{
    [TestFixture]
    public class CreateJobRequestTests
    {
        [Test]
        public void EnabledProperty_ShouldDefaultToTrue()
        {
            // Act
            var request = new CreateJobRequest();

            // Assert
            request.Enabled.Should().BeTrue();
        }
    }
}

