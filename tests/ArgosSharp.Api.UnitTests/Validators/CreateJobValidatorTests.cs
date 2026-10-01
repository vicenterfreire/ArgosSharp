using ArgosSharp.Api.Validators;
using ArgosSharp.Application.Contracts.Jobs;
using ArgosSharp.Domain.Enums;
using ArgosSharp.Domain.ValueObjects;
using FluentAssertions;

namespace ArgosSharp.Api.UnitTests.Validators
{
    [TestFixture]
    public class CreateJobValidatorTests
    {
        private CreateJobValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new CreateJobValidator();
        }

        [Test]
        public void Validate_WhenParametersIsNull_ShouldHaveValidationError()
        {
            // Arrange
            var request = new CreateJobRequest
            {
                Name = "Test Job",
                JobType = JobType.NewsArticles,
                Parameters = null!,
                Priority = JobPriority.Normal,
                Enabled = true
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error =>
                error.PropertyName == nameof(CreateJobRequest.Parameters)
                && error.ErrorMessage == "Parameters are required.");
        }

        [Test]
        public void Validate_WhenParametersIsNotNull_ShouldPassValidation()
        {
            // Arrange
            var request = new CreateJobRequest
            {
                Name = "Test Job",
                JobType = JobType.NewsArticles,
                Parameters = new JobParameters("Some Test", ["https://example.com"], 1),
                Priority = JobPriority.Normal,
                Enabled = true
            };

            // Act
            var result = _validator.Validate(request);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}