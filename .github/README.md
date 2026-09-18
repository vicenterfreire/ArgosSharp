# GitHub Copilot workspace setup

This repository includes a small set of GitHub Copilot chat mode definitions under [.github/chatmodes](chatmodes) to organize work by role.

## Available roles

- Code Reviewer: validates correctness, architecture, and maintainability.
- Developer: implements scoped changes within repo conventions.
- Architect: evaluates system boundaries and long-term design.
- Tester: writes and repairs tests around real behavior.
- Technical Writer: produces specs and project documentation.
- Orchestrator: coordinates the work across specialist roles.

## How to use them

Open the GitHub Copilot chat UI in VS Code and select the relevant chat mode from the available modes. Use the agent that matches the task at hand.

## Repository guidance

For this project, keep the following in mind:

- prefer the current code as the source of truth over stale assumptions,
- keep changes aligned with the layered architecture,
- validate with the smallest relevant test command,
- preserve evidence-backed conclusions.

## Related files

- [chatmodes/code-reviewer.chatmode.md](chatmodes/code-reviewer.chatmode.md)
- [chatmodes/developer.chatmode.md](chatmodes/developer.chatmode.md)
- [chatmodes/architect.chatmode.md](chatmodes/architect.chatmode.md)
- [chatmodes/tester.chatmode.md](chatmodes/tester.chatmode.md)
- [chatmodes/technical-writer.chatmode.md](chatmodes/technical-writer.chatmode.md)
- [chatmodes/orchestrator.chatmode.md](chatmodes/orchestrator.chatmode.md)
