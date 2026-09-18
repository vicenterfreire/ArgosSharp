# Copilot instructions for ArgosSharp

## Project context

This repository is organized in layers:

- Domain
- Application
- Infrastructure
- API
- Tests

Keep implementation aligned with this structure and avoid introducing cross-layer shortcuts.

## Core expectations

- Prefer the current source code as the source of truth.
- Preserve unrelated work and avoid broad refactors.
- Keep changes small and scoped to the requested task.
- Use the repository's current test and naming conventions.
- Validate with the smallest relevant command before concluding work.

## Testing guidance

- Use NUnit, Moq, and FluentAssertions where appropriate.
- Follow the existing AAA pattern.
- Use names following the Method_Condition_Expected convention.
- Focus on real behavior rather than mock-only assertions.

## Architecture guidance

- Do not broaden responsibility across layers.
- Respect domain invariants and project boundaries.
- Maintain dependency direction consistent with the established architecture.
- Separate implementation from review and documentation tasks unless explicitly requested.

## Evidence requirement

When reporting results, prefer evidence-backed statements tied to the actual code or validation output. Do not claim completion without a relevant test or verification result.
