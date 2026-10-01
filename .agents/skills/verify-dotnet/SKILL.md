---
name: verify-dotnet
description: Use when verifying builds, automated tests, and changes in the ArgosSharp .NET solution.
---

# ArgosSharp Verification

1. Identify the solution and test projects.
2. Inspect repository scripts and build configuration.
3. Restore dependencies only when needed and permitted.
4. Build the relevant projects.
5. Run relevant automated tests.
6. Investigate failures instead of suppressing them.
7. Inspect the final diff for accidental modifications.
8. Report exact commands and actual results.

Do not run destructive commands or access production infrastructure.
Do not report checks as successful when they were skipped.
