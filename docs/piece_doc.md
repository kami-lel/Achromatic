# Piece Documentation

Piece prefab:

- place Piece Prefab's Transform at: where player should be when main part of main piece start

- set `StartPreludeTrigger`'s Capsule collider: capture entire area where prelude should be played
- set `StartMainPieceTrigger`:

  - its collider: where start to finish the prelude
  - must be inside of `StartPreludeTrigger`

<!-- todo -->