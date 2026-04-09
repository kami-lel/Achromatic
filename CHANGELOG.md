# Achromatic CHANGELOG

> USC CTIN-532 Project

[^format]

<!-- Todo make web / mac build -->
<!-- Todo particle efx: on monster killed,jumped,etc. -->
<!-- Todo particle efx: running score -->
<!-- Todo add schools logo -->
<!-- Fixme squat obstacle (also for jump) confusing with background -->
<!-- Todo write dev log -->

<!-- bug fix sprite -->
<!-- todo smashing inputs type -->
<!-- todo pause screen, allow restart/resume -->
<!-- todo show player character origin in world -->
<!-- todo set up hooks utility -->
<!-- todo Wwise Unity Integration -->
<!-- todo add map dynamic response to player performance -->
<!-- fixme Lv2 music need boost volume of drum track -->
<!-- fixme visual feedback for holding left stick -->
<!-- fixme title screen need better font -->
<!-- fixme camera movement during title screen -->














## [Unreleased]

### Added
### Changed
### Deprecated
### Removed
### Fixed

## 1.0.0 Release

## 1.0.0-beta.2 Beta2 Milestone

## [1.0.0-beta] Beta Milestone

### Added

- obstacles & enemy (mob) during music play
- functional parallax background
- Lv1 distant background image
- basic particle effects during player running
- scene transition logic with fade-in and fade-out
- enemy prefab can be killed

### Changed

- using a new tile sets
- re-create the entire Lv1 map
- floating objects in the background using particle system

### Fixed

- Camera Jittering, by fine tunning virtual camera parameters


































## [1.0.0-alpha] Alpha Milestone - 2026-03-26

### Added

- Hit Type Indicator to tell judge result
- implement Credits Scene with placeholder content
- piece ending logic

### Changed

- refactorization of Piece and Player, using multiple components approach
- improve Game Title movement to be smooth & natural
- flatten down Beatmap Note structure, directly use string-typed note typed
- reimplement score indicators
- fine tunning player jumping animation curve during music play
- refactorization of SFX script
- layered audio sfx: audio cue reflect both action & hit type, and random use one of few samples
- re-implement background with static background color & dynamic floating objects
- create an interesting beatmap for Lv1

### Removed

- un-used Assets

### Fixed

- player sprite transparent issue













## [0.9.1] Pre-Alpha - 2026-03-12

### Added

- FPS Counter
- various gameplay indicators:

  - Running Score Indicator
  - Score Addition Indicator
  - Combo Indicator
  - Hit Type Indicator

- 2 new player actions: Squat & Attack
- improve player animation/actions during main script

### Changed

- `PlayerScript.cs` code refactorization, utilize various managers
- fine tunning camera for better play experience
- using Controller Rumble to provide feedback information
  of both hit type and action type during music play

### Removed

- unused sprites from last build















## [0.9.0] Pre-Alpha - 2026-03-05

### Added

- implement `pseudoAudioPlugin.cs`: temporary audio controller before finalize which audio software to use
- using `Spline` package to manage player's path during main piece play
- using Cinemachien to control the camera:

  - implement zoom out during music play

- in-map Game Title
- a generic `PrefabPool` that can be used during Piece, etc.

### Changed

- code/scripts refactorization: break down `PieceScript.cs` into multiple classes
- allows setting judge timing as Scriptable Object
- use a single GameController singleton for manage game states across scripts













## [0.5.1] - 2026-02-26













## [0.5.0] Vertical Slice - 2026-02-18














## [0.5.0-beta] - 2026-02-18

## [0.3.1] - 2026-02-01

In this iteration of the game/toy, I am trying to explore the possibility of combining a music game with a 2D platformer RPG-style game. The player is able to explore the world a little bit, then is transported to play the game. In this way, there is the possibility of narrative building that can echo the theme and lyrics of the song. I am also adding more visual and audio feedback to the game to help the player understand if they are playing well.
















## [0.3.0] - 2026-02-01

In this iteration of the game/toy, I am trying to explore the possibility of combining a music game with a 2D platformer RPG-style game. The player is able to explore the world a little bit, then is transported to play the game. In this way, there is the possibility of narrative building that can echo the theme and lyrics of the song. I am also adding more visual and audio feedback to the game to help the player understand if they are playing well.














## [0.2.0] - 2026-01-25

## [0.1.0] - 2026-01-22

















[unreleased]: https://github.com/kami-lel/usc-ctin532-game-project/compare/v1.0.0-beta...dev
[1.0.0-beta]: https://github.com/kami-lel/usc-ctin532-game-project/compare/v1.0.0-alpha...v1.0.0-beta
[1.0.0-alpha]: https://github.com/kami-lel/usc-ctin532-game-project/compare/v0.9.1+pre_alpha...v1.0.0-alpha
[0.9.1]: https://github.com/kami-lel/usc-ctin532-game-project/compare/v0.9.0+pre_alpha...v0.9.1+pre_alpha
[0.9.0]: https://github.com/kami-lel/usc-ctin532-game-project/compare/v0.5.1...v0.9.0+pre_alpha
[0.5.1]: https://github.com/kami-lel/usc-ctin532-game-project/compare/v0.5.0+vertical_slice...v0.5.1
[0.5.0]: https://github.com/kami-lel/usc-ctin532-game-project/compare/v0.5.0-beta...v0.5.0+vertical_slice
[0.5.0-beta]: https://github.com/kami-lel/usc-ctin532-game-project/compare/v0.3.1...v0.5.0-beta
[0.3.1]: https://github.com/kami-lel/usc-ctin532-game-project/compare/v0.3.0...v0.3.1
[0.3.0]: https://github.com/kami-lel/usc-ctin532-game-project/compare/v0.2.0...v0.3.0
[0.2.0]: https://github.com/kami-lel/usc-ctin532-game-project/compare/v0.1.0...v0.2.0
[0.1.0]: https://github.com/kami-lel/usc-ctin532-game-project/compare/v0.1.0













[^format]: CHANGELOG format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/); Version scheme adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).