# MobileGameProject

A deliberately small Unity 6 teaching template for short mobile microgames.

## Open the example

Open `Assets/Scenes/MobileGameTemplate.unity`, then enter Play mode.

The demonstration flow is:

1. Choose **START** on the menu.
2. Wait for the countdown.
3. Press **WIN** before the five-second timer expires.
4. Choose **REPLAY** or **MENU** on the result screen.

If the timer reaches zero, the result is failure.

## Framework scripts

- `MicrogameState.cs` defines the shared states.
- `MicrogameBase.cs` is the base class for new microgames.
- `MicrogameManager.cs` controls countdown, timer, and results.
- `MicrogameUI.cs` controls the three UI panels.
The framework scripts live under `Assets/Scripts/Framework`.

The separate example game lives under
`Assets/Scripts/Microgames/Placeholder/PlaceholderMicrogame.cs`. In the scene,
`MicrogameFramework` contains only the shared manager, while
`PlaceholderGamePanel` contains the example component and its WIN-button UI.

The scene-building utility is intentionally kept in `Assets/Editor` so tutors can use
**Tools > Mobile Game Template > Rebuild Demo Scene** if the example scene needs to be recreated.

This project was generated with Unity 6000.0.83f1 using APIs compatible with Unity 6 / 6000.3.x.


