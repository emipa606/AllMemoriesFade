# GitHub Copilot Instructions for RimWorld Mod: All Memories Fade

## Mod Overview and Purpose

**Mod Name:** All Memories Fade  
**Author:** Mlie  
**PackageId:** Mlie.AllMemoriesFade

The "All Memories Fade" mod is designed to alter the way emotional memories affect the mood of pawns in RimWorld. Traditionally, certain memories such as divorces or breakups would cause significant mood debuffs for a fixed period before disappearing abruptly. This mod introduces a dynamic fading mechanism where all timed memories, both positive and negative, gradually decrease in intensity until they fade away completely. This leads to a more emotionally stable environment for the colonists. The gradual fading is based on the remaining time duration of the memory effect, ensuring smoother transitions and reducing mental strain on pawns over time.

## Key Features and Systems

- **Incremental Memory Fading:** All emotional memories fade gradually, reducing intense mood swings and improving overall pawn emotional stability.
- **Configurable Settings:** 
  - Adjust the base mood-values for memories, allowing customization of emotional impact.
  - Modify the duration settings for timed thoughts.
  - Exclude specific duration memories from the fading process.
- **Performance-Friendly:** Utilizes existing vanilla RimWorld features introduced in version 1.3, ensuring no additional performance overhead.
  
## Coding Patterns and Conventions

- **C# File Structure:** The mod comprises three main C# files:
  1. `AllMemoriesFadeSettings.cs`: Manages user settings with methods like `ExposeData` and `ResetSettings`.
  2. `AllMemoriesFade.cs`: Handles core functionalities, including memory effects and length adjustments.
  3. `AllMemoriesFadeMod.cs`: Integrates with RimWorld's mod settings interface, providing methods such as `DoSettingsWindowContents` and `WriteSettings`.

- **Class Naming:** Follows PascalCase convention for class and method names.
- **Settings Management:** Utilizes settings management methods to handle user preferences efficiently.

## XML Integration

Currently, there are no XML files utilized in this mod. All functionalities and configurations are handled directly in the C# codebase.

## Harmony Patching

The mod benefits from 1.3 vanilla implementations for memory effects, thus not requiring additional Harmony patches. This integration ensures compatibility and reduces the risk of performance issues or conflicts with other mods.

## Suggestions for Copilot

- **Code Suggestions:** Provide efficient algorithms for calculating the fading effect based on remaining time, ensuring smooth transition of mood values.
- **Settings Management:** Assist with robust and user-friendly settings management to ensure easy customization by players.
- **Performance Monitoring:** Optimize existing methods for minimal performance impact, ensuring seamless gameplay experiences for users.

By following the documented structure and providing consistent contributions, developers can effectively extend and enhance the "All Memories Fade" mod to further improve RimWorld gameplay experiences.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

