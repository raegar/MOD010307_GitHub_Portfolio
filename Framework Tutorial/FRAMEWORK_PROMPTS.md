# Build the framework with Copilot in small steps

Use Copilot Chat in VS Code with the Unity project folder open. Make a branch named `framework/scene-navigation`. Paste **one** prompt at a time. Read the proposed diff before accepting it. If Copilot changes files outside the requested scope, reject that part. After each stage, return to Unity, wait for compilation and make one descriptive commit. Run the interactive checks after the tutorial has supplied the required scene objects and button wiring.

Tell Copilot which stage you are on. Do not ask it to create the whole project, all four games or the worked practice game. Keep its replies in the pull request evidence. Exact wording may vary, but preserve the file scope and acceptance check.

## Stage 0 — create the project yourself

In Unity Hub, create a Unity 6.3 project. Open it once. Make `Assets/Scenes`, `Assets/Scripts/Framework` and `Assets/Scripts/Microgames/PracticeTap`. Save two blank scenes as `MainMenu` and `PracticeTap`. Open **File > Build Profiles > Platforms > Scene List**; add both and put `MainMenu` first. Do not use Copilot yet. Check both scene files and the Scene List before moving on.

## Stage 1 — scene navigation

> We are building a small Unity 6.3 C# microgame framework. Follow this specification: MainMenu and each microgame are separate scenes, listed in Build Profiles. Create only `Assets/Scripts/Framework/SceneNavigator.cs` in namespace `MicrogameCourse.Framework`. It must use `SceneManager.LoadSceneAsync` with `LoadSceneMode.Single`; expose public no-argument `OpenPractice`, `OpenMenu` and `Replay` methods for uGUI Button On Click events; ignore repeated clicks while loading; and log a useful error if a named scene is absent from the build scene list. The scene names are exactly `MainMenu` and `PracticeTap`. Do not create scenes, game rules, singletons, persistent managers, or other files. Explain each method in plain language and list two manual tests.

Review: Which exact scene names appear in the code? Does the file avoid `DontDestroyOnLoad`? Does it use Single mode? Compile the script now. After tutorial step 3 wires the menu button, run F1. After step 6 wires the game Menu button, run F8.

Commit example: `Add single-scene navigation between menu and practice`.

## Stage 2 — game contract and compiling bridge

> Create only two files under `Assets/Scripts/Framework` in namespace `MicrogameCourse.Framework`. `MicrogameBehaviour.cs` is an abstract `MonoBehaviour` that stores its `MicrogameSession` in a protected `Session` property, tracks whether it is running in a protected `IsRunning` property, has public virtual `Begin(MicrogameSession session)` and `End()` methods, and protected `Win()` and `Lose()` methods that call `Session.Finish(bool)` only while running. Create `MicrogameSession.cs` as a temporary, minimal `MonoBehaviour` with a public `Finish(bool won)` method that logs the result to the Console; the next stage will replace this placeholder method with the full flow. Both files must compile together. Do not write game rules, UI or navigation. Explain what a student game must call and what it can override.

Review: Is `Session` inaccessible to unrelated components? Can a stopped game report a second result? Can you explain inheritance and `base.Begin(session)`? Compile both files before proceeding. The temporary `Finish` method is only a bridge; do not use it as the finished session.

Commit example: `Define minimal microgame lifecycle contract`.

## Stage 3 — one-scene session flow

> Update only the existing `Assets/Scripts/Framework/MicrogameSession.cs` in namespace `MicrogameCourse.Framework`. Replace its temporary Console-only `Finish` with the complete session flow. Use the existing `MicrogameBehaviour` contract. Inspector references: game, readyPanel, playArea, resultPanel, timerText, resultText and durationSeconds. Use Unity uGUI `Text` for this exercise. Flow: Ready on Awake; public no-argument `StartGame()` activates the play area, begins the game and starts a timer; Update counts down only while Playing; public `Finish(bool won)` accepts exactly one result, calls End, and shows either `You win!` or `Time is up!`. Keep this limited to the current game scene. Do not create game rules, scene assets, a score system or new files. Explain which fields must be dragged into the Inspector and provide a timeout test.

Review: Does `StartGame` reject a second click? Does `Finish` reject a second result? Does the session call `game.Begin(this)` once? Compile now. After tutorial steps 4–6 create and wire the UI and practice game, run F2–F6.

Commit example: `Add ready play result flow for one microgame scene`.

## Stage 4 — inspect and simplify

> Review only the three framework scripts against `FRAMEWORK_SPEC.md`. Find mismatches or unnecessary features, with file and line references. Suggest the smallest corrections. Do not edit files or generate microgame code. Prioritise scene loading, repeat clicks, timer completion and replay behaviour.

Apply only corrections you understand. Do this stage after finishing the practice tutorial. Re-run F1–F9 and record expected versus actual results. Open a framework pull request with the prompt history, useful AI output, changes you made and validation evidence.

## When adding each assessed game

Create `Game01.unity` or the next numbered scene yourself, add it to Build Profiles, and add one named menu entry method to `SceneNavigator`. Copy only the **scene structure** you need. Write the game rules yourself. Ask Copilot for documentation lookup, error explanation, review or test ideas, but do not ask it to generate the core game implementation that you will claim as Learning Outcome 3 evidence.
