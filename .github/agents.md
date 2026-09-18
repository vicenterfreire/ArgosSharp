# Agent roles summary

This repository contains a small set of GitHub Copilot role definitions to help coordinate work.

## Role map

- Code Reviewer: checks quality and correctness.
- Developer: implements the fix or feature.
- Architect: evaluates system design and boundaries.
- Tester: creates or repairs validation coverage.
- Technical Writer: maintains docs and specs.
- Orchestrator: manages the workflow across roles.

## Recommended usage

- Use the Orchestrator when the task spans multiple concerns.
- Use the Developer for implementation work.
- Use the Tester when the task includes validation or regression protection.
- Use the Code Reviewer for verification, design review, or risk analysis.
- Use the Architect for structural decisions or architecture changes.
- Use the Technical Writer for README, specs, ADRs, or internal documentation.

## Short rules

- Follow the repository structure.
- Prefer current code and contracts over stale assumptions.
- Keep changes minimal and relevant.
- Use evidence before claiming success.
