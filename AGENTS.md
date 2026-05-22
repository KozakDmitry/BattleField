# AGENTS.md

Unity **6000.3.10f1** — URP blank template project.

## Code architecture

- **Custom DI** (`DIContainer`, static helper `DI`) — not Zenject/VContainer. Register: `DI.Register<T>(instance)`, resolve: `DI.ResolveSync<T>()` / `DI.ResolveAsync<T>()`. Supports `RegisterMode.global` / `RegisterMode.scene`; scene-scoped registrations drop on scene unload.
- **State machine** (`GameStateMachine`) with states: `BootstrapState → LoadProgressState → LoadLevelState → GameLoopState`. Interfaces: `IState`, `IPayloadedState<TPayload>`.
- **Two-phase init**: `SetupController` calls `Initialize()` then `AfterInitialize()` on all `InitializableWindow` children.
- **Input**: `TouchControls.inputactions` (generated `TouchControls.cs`). Wrapped behind `IInputService` — do not reference generated class directly in game code.
- **Async**: `UniTask` (Cysharp), not `async/await` with `Task`.
- **Tweening**: DOTween (Demigiant). `DOTWEEN` scripting symbol defined for all platforms.
- **Addressables**: custom auto-generated enum mapping. Regenerate via `Tools → Addressables → Generate Group Mapping`.
- **Save**: `PlayerPrefs` + `JsonUtility` — no binary serialization.
- **Build order**: `Splash.unity` → `Loading.unity` → `Game.unity`.
- **URP**: separate PC and Mobile render pipeline assets in `Assets/Project/Settings/`.

## Namespace quirks

The project has inconsistent namespace casing. Prefer `Assets.Project.CodeBase.*` but some files use `Assets.Project.Codebase.*` (lowercase `b`) or `Assets.Project.Scripts.*`. Existing code in the folder you're editing dictates the convention — match it.

## Commands & workflow

- **Tests**: no test framework or test files exist in the project. Do not add tests without explicit request.
- **No codegen steps** (Addressables enum generation is manual via menu).
- **SceneTransition** (`SceneTransition.cs`) handles crossfade loading. Use `SceneService` to load scenes, not `SceneManager` directly.
- **`StaticDataService`** is a static class with `ConcurrentDictionary` cache — safe for concurrent Addressable loads.

## What to avoid

- Do not add Zenject, VContainer, or other DI frameworks.
- Do not use `async Task` — use `UniTask` everywhere.
- Do not reference the generated `TouchControls` class directly outside `InputService`.
- Do not add tests unless asked — there are zero test files in the repo.
- Do not reference `Menu` scene — it's defined in `StaticVariables.cs` but has no `.unity` file.
