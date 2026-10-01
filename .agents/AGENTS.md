# ArgosSharp — Shared Engineering Context

## Project purpose

ArgosSharp is a .NET backend application for scheduled and
on-demand scraping jobs, execution tracking, and artifact delivery.

## Known architecture and technologies

- Target framework: .NET 8, unless the repository says otherwise.
- Follow the existing Clean Architecture boundaries.
- Job represents the configured task.
- JobExecution represents an individual execution.
- PostgreSQL stores application data and artifact metadata.
- Background processing integrates with AWS SQS.
- S3 stores generated artifacts.
- Failed messages may be retried and eventually routed to a DLQ.
- NUnit and FluentAssertions are used for automated tests where
  configured by the existing solution.

Treat this list as initial context, not proof that every component
is currently implemented. Inspect the repository before changing it.

## Architecture constraints

- Domain must not depend on infrastructure implementations.
- Application coordinates use cases and business workflows.
- Infrastructure contains persistence, messaging, storage,
  and external integrations.
- API projects expose application capabilities without duplicating
  business rules.
- Preserve existing dependency direction and project references.
- Do not introduce CQRS, repositories, factories, or abstractions
  without a concrete requirement.

## Execution and messaging

When working on background execution, investigate:

- Message acknowledgement and failure behavior.
- Retry count and dead-letter routing.
- Idempotency and duplicate delivery.
- Execution status transitions.
- Cancellation and graceful shutdown.
- Database consistency and partial failures.
- Correlation between JobExecution and stored artifacts.

Do not assume these behaviors are already correct. Verify them.

## Persistence and artifacts

- Inspect existing EF Core mappings and migrations before changes.
- Preserve database compatibility.
- Consider transaction boundaries and concurrent updates.
- Keep artifact metadata consistent with object storage operations.
- Do not delete stored objects or alter existing data without
  explicit authorization.

## Testing expectations

- Use the test framework and conventions already in the solution.
- Prefer testing domain rules without infrastructure dependencies.
- Test application workflows and external integration boundaries.
- Include failure, retry, duplicate-message, and concurrency cases
  when relevant to the changed behavior.
- Do not claim coverage percentages without measurement.

## Git and safety

- Inspect git status before editing.
- Preserve uncommitted user changes.
- Never push to a remote repository.
- Never commit without explicit authorization.
- Do not run destructive database, cloud, or infrastructure commands
  without explicit approval.

## Completion report

Every non-trivial task must report:

1. What changed and why.
2. Files modified.
3. Build and test commands actually executed.
4. Passed, failed, and skipped checks.
5. Architectural implications and unresolved risks.
