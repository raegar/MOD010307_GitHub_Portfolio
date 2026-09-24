# Microgame framework teaching pack

This pack supports the 2026–27 MOD010307 assignment. Students use a coding assistant to build and review a small shared framework, one checked step at a time. They then type, connect, test and explain the worked `PracticeTap` game themselves. The practice game teaches the integration process; students must develop their four assessed microgames independently.

## Start here

1. Read [the framework specification](FRAMEWORK_SPEC.md).
2. Follow [the staged Copilot prompts](FRAMEWORK_PROMPTS.md) in a new Unity 6.3 project. Stop and test after each prompt.
3. Follow [the first game tutorial](FIRST_GAME_TUTORIAL.md) without asking AI to write the game code.
4. Use `UnityProject` as a tutor reference, not as a student starter to submit unchanged. In Unity, choose **Tools > Microgame Course > Build Reference Scenes** once to create the two example scenes. Then open `MainMenu` and press Play.

The old generated example remains in `Templates/MOD010307_GitHub_Portfolio` outside this folder. This pack uses **one scene per microgame** and loads scenes in `Single` mode. `ReadyPanel` and `ResultPanel` are local to one game scene; the four games never share one crowded scene.

## Teaching sequence

| Session | Student creates | Checkpoint |
|---|---|---|
| 1 | Unity project, folders, two empty named scenes | Both scenes appear in the Scene List |
| 2 | Framework scripts and menu scene | All three framework prompts compile; menu opens the practice scene |
| 3 | Practice scene UI and hand-coded game | Five taps wins; timeout loses; replay and menu work |
| 4 | Framework review | F1–F9 checked and prompt history recorded |
| Later | Four distinct assessed microgames | Separate scene, scripts, tests and evidence for each |

## Files

- `FRAMEWORK_SPEC.md`: fixed behaviour, scene ownership and acceptance criteria.
- `FRAMEWORK_PROMPTS.md`: bounded prompts, review questions and checkpoints.
- `FIRST_GAME_TUTORIAL.md`: beginner instructions and code walkthrough.
- `VALIDATION.md`: observed tutor reference checks and their limits.
- `UnityProject`: tutor reference scripts and a scene creation utility.

The tutor scene utility exists to inspect the finished reference. Students should construct their scenes using the tutorial and should not use it as a substitute for the staged work.
