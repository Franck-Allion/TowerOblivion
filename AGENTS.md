# Project Instructions

## Context7 MCP Usage

Use the `context7` MCP server when current or version-specific documentation would improve accuracy, especially for:

- Library, framework, SDK, API, or tool usage questions.
- Setup, configuration, migration, or upgrade steps.
- Code generation that depends on third-party APIs or current package behavior.
- Debugging errors where upstream docs, examples, or configuration options may have changed.

When using Context7:

- Resolve the package or library name to a Context7 library ID first.
- Fetch the relevant documentation before giving implementation guidance or editing code that depends on that external API.
- Prefer Context7 documentation over memory for modern dependencies.
- If Context7 is unavailable or does not contain the needed library, state that briefly and use the next best source.

- Do not use Context7 for purely local codebase questions, general language syntax, or project-specific behavior that can be answered from repository files.

## Unity Naming Conventions

All AI agents MUST adhere to the following conventions when creating or modifying Unity objects:

- **GameObjects**: Use `PascalCase`. UI elements must use functional suffixes: `Button`, `Label`, `Text`, `Panel`, `Input`.
- **UI Children**: Child TextMeshPro objects in buttons/containers should be named exactly `Text`.
- **Localization**: Keys must use `PascalCase` with dot notation (e.g., `Category.SubCategory.KeyName`).
- **Scene Integrity**: Never create scenes without a `Main Camera`, `Global Light 2D`, and `EventSystem`.
- **Singletons**: Use the `GlobalBootstrapper` auto-creation pattern for accessing global services.

