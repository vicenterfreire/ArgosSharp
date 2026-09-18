# ArgosSharp knowledge base

## Repository shape

This solution follows a layered architecture with clear responsibilities:

- Domain: value objects, entities, enums, factories, and invariants.
- Application: use cases, services, contracts, and orchestration logic.
- Infrastructure: scraper implementations and infrastructure adapters.
- API: HTTP entry points that expose the application layer.
- Tests: project-per-layer NUnit suites with Moq and FluentAssertions.

The source tree is organized under src/ and tests/, which matches the actual project structure in the repository.

## Key domain rules

### Job parameters

The current repository contract for JobParameters is defined in `src/ArgosSharp.Domain/ValueObjects/JobParameters.cs`.

Observed facts:

- `query` cannot be null.
- `sources` cannot be null and must contain at least one value.
- `depth` is validated with `ArgumentOutOfRangeException.ThrowIfLessThan(1, depth, nameof(depth));`.
- This means the real minimum valid depth is 1, not 2.

This was a recurring source of false failures in the application tests when stale test data used depth 2 or other assumptions from older code.

### Job and execution status flow

The domain model supports a job lifecycle that is exercised by the app and infra tests:

- A `Job` is created with name, type, parameters, priority, and enabled state.
- A `JobExecution` tracks status and job parameters for processing.
- State transitions are handled by the domain execution flow and then consumed by the job processor service.

## Application layer patterns

### ScraperContext

The `ScraperContext` in `src/ArgosSharp.Application/UseCase/Scraper/ScraperContext.cs` resolves existing scraper strategies by source name.

Important behaviors:

- It builds dictionaries keyed by scraper `Name`.
- It enumerates `jobParameters.Sources` and looks each one up in the dictionary.
- It calls `ProcessScraperAsync(jobParameters.Query, jobParameters.Depth)` for each selected source.

This means tests must configure `Name` on each mock and use valid source keys that exist in the strategy dictionaries.

### Use case responsibilities

- `CreateJobUseCase` creates a job via `IJobFactory` and persists it via `IJobRepository`.
- `GetJobUseCase` retrieves jobs from the repository.
- `JobProcessorService` executes a queued job execution, fetches the matching job, invokes the scraper context, exports the result, and creates an artifact.

## Infrastructure layer notes

The infrastructure project contains scraper implementations for different regions/sources, such as news and job-posting sources. Each scraper follows a common pattern:

- implement a scraper interface for the relevant content type,
- expose a `Name` property for source matching,
- implement `ProcessScraperAsync(query, depth)`.

The test suite validates these behaviors with real content shapes and common failure conditions.

## Testing conventions used in the repo

The solution consistently uses:

- NUnit as the test framework.
- Moq for dependency mocks.
- FluentAssertions for assertions.
- AAA style: Arrange, Act, Assert.
- Method_Condition_Expected naming pattern for test methods.

This is important when adding or repairing tests, because the repository already follows those conventions across layers.

## Lessons learned while working in the repo

1. Always trust the current source code over stale test assumptions.
2. Domain validation rules are the real contract; tests must match them.
3. Strategy dictionaries rely on exact source names, so mock setup must include `Name` values.
4. JobParameters depth should be treated as a real-value contract, not a free-form number.
5. Layer-by-layer verification is effective: domain, infrastructure, then application.
6. The repository has a strong pattern of project-specific conventions that should be preserved when writing or fixing tests.

## Useful files to inspect first

- `src/ArgosSharp.Domain/ValueObjects/JobParameters.cs`
- `src/ArgosSharp.Application/UseCase/Scraper/ScraperContext.cs`
- `src/ArgosSharp.Application/Services/JobProcessor/JobProcessorService.cs`
- `src/ArgosSharp.Application/UseCase/Jobs/CreateJob/CreateJobUseCase.cs`
- `tests/ArgosSharp.Application.UnitTests/UseCase/CreateJobUseCase.UnitTests.cs`
- `tests/ArgosSharp.Application.UnitTests/StrategiesContext/ScraperStrategyContext.UnitTests.cs`

## Practical takeaway

The main mistake to avoid in future work is assuming older depth values or outdated source contracts from stale tests. The current implementation is the source of truth, and the test suite should be aligned to it before adding new scenarios.
