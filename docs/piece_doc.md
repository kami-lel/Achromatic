# Piece Documentation

## Piece Prefab Usage

- add Piece Prefab under Scene's root level
- place Prefab's Transform at: where player should be when main part of the piece start, i.e. end of prelude
- move `StartVampTrigger`'s collider: capture entire area where music vamp is playing.
- set `StartPreludeTrigger`'s collider:

  - when touched, start routine of starting main piece
  - must be inside of `StartPreludeTrigger`e

## transition from exploration to rhythmic:

1. exploration

2. start **vamp** when: player enters `StartPreludeTrigger`,
   vamp will be looping, and wait player to start prelude

3. enters `StartPreludeTrigger` when vamp is playing:

  - player lose control of the character, it will automatically walk toward main piece start point
  - music's prelude start playing
  - player prepare themselves for the rhythmic part

4. when prelude is finished, main music start as rhythmic