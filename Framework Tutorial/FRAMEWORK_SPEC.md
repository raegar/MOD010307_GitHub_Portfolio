# Shared microgame framework specification

## Learning purpose and boundary

The framework provides a consistent way to start, finish, replay and leave a microgame. AI assistance is permitted for framework and integration work when it is declared and checked. The four assessed microgames and code submitted as direct Learning Outcome 3 evidence must be student authored. The worked practice game in this pack is a guided exercise, not one of the four assessment games.

## Required structure

```text
Assets/
  Scenes/
    MainMenu.unity
    PracticeTap.unity
    Game01.unity ... Game04.unity (created later)
  Scripts/
    Framework/
      SceneNavigator.cs
      MicrogameBehaviour.cs
      MicrogameSession.cs
    Microgames/
      PracticeTap/TargetTapGame.cs
      Game01/ ... Game04/ (student-authored later)
```

Each game has its **own Unity scene**, Canvas, game objects and one `MicrogameSession`. `MainMenu` is a separate scene. Use `SceneManager.LoadSceneAsync(..., LoadSceneMode.Single)` so the previous scene is unloaded when the next opens. Do not add all games as hidden panels in one scene. A game scene may have its own ready and result panels.

## Fixed interface

`SceneNavigator` exposes `OpenPractice`, `OpenMenu` and `Replay` as public no-argument methods for Button `On Click` events. Add equivalent named entry methods as new games are introduced. It rejects unknown scenes with a clear Console error and guards against repeat taps while loading.

`MicrogameBehaviour` is an abstract `MonoBehaviour` with `Begin(MicrogameSession session)`, `End()`, `IsRunning`, and protected `Win()` and `Lose()`. It is the only framework type a game script must inherit. Each game decides its own rules and calls `Win()` or `Lose()` once.

`MicrogameSession` owns a three-phase flow: `Ready → Playing → Result`. Its Inspector fields are one game component, ready panel, play area, result panel, timer text, result text and duration. The Start button calls `StartGame()`. The timer runs only while Playing. A result can occur once. Replay reloads the current scene; Menu loads `MainMenu`.

Use uGUI buttons for the practice exercise so mouse clicks in the editor and taps on a device use the same path. Configure the Canvas Scaler to **Scale With Screen Size**, reference resolution **1080 × 1920** and match **0.5**. Use portrait Game view for initial checks. On device, check that buttons remain legible and inside safe screen bounds.

## Acceptance checks

| ID | Action | Expected result |
|---|---|---|
| F1 | Play `MainMenu`; press Practice | `PracticeTap` becomes the active scene; `MainMenu` unloads. |
| F2 | Enter `PracticeTap` | Ready instructions and Start appear; play area and result are hidden. |
| F3 | Press Start once | Game begins, timer starts near 10 seconds. |
| F4 | Press Start repeatedly | No second run or duplicate timer starts. |
| F5 | Call `Win()` before timeout | Timer stops, game input stops, result says `You win!`. |
| F6 | Allow time to expire | Result says `Time is up!`; late taps cannot change it. |
| F7 | Press Replay | Current scene reloads in Ready state with fresh game state. |
| F8 | Press Menu | `MainMenu` loads and game scene unloads. |
| F9 | Omit a scene from Build Profiles | Console identifies the missing scene; the existing scene stays open. |

## Scope limit

This version has no global score, save file, singleton, persistent object, scene registry or transition animation. Add one only when a game requirement justifies it. Four games should be integrated by repeating a small scene pattern, rather than increasing framework complexity.

## Assessment evidence

For each framework change, record the prompt, useful response, changes made and validation result in the framework pull request. Keep a screenshot or short clip of the Scene List and a menu-to-game-to-menu run. For each student-authored game, keep its own branch and pull request, identify the authored scripts, record expected and actual test results, and explain the code. The assessment brief also requires a touch or gesture led game, testing of all four games, and a measured performance improvement.

## Unity references

- [Scene loading API](https://docs.unity3d.com/6000.3/Documentation/ScriptReference/SceneManagement.SceneManager.LoadSceneAsync.html)
- [Manage scenes in builds](https://docs.unity3d.com/6000.3/Documentation/Manual/build-profile-scene-list.html)
- [Button API](https://docs.unity3d.com/Packages/com.unity.ugui@2.0/api/UnityEngine.UI.Button.html)
