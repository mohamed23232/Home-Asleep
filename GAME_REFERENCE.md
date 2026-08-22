# Home & Asleep — Game Reference

## Overview

**Home & Asleep** is a 2D platformer where the core mechanic is switching between an **Awake** state and an **Asleep** (dream) state. Platforms, passages, and obstacles exist in one state but not the other, so the player toggles between them to navigate levels and solve puzzles. The win condition for each level is collecting 3 stars and reaching the exit trigger.

---

## Controls

| Action | Keyboard | Gamepad |
|---|---|---|
| Move | WASD / Arrow Keys | Left Stick / D-Pad |
| Jump | Space | A (South) |
| Dash | Left Shift | B (East) |
| Interact | E | Y (North) |
| Switch Awake/Asleep | X | RT (Right Trigger) |

Mobile controls are shown automatically on handheld/mobile platforms and hidden on desktop.

---

## Scene Order

1. Main Menu
2. How To Play
3. Level 1 (with opening cutscene)
4. Level 2, 3 … (each references the next via `nextLevel` string)
5. Final Level (triggers ending cutscene on completion)
6. Credits

`BackgroundMusic.cs` persists across scenes (DontDestroyOnLoad) and switches between a UI track (Main Menu, Credits) and a level track (all gameplay scenes).

---

## Awake / Asleep Switching

The central system. Pressing the Switch input toggles `isAwake` on `PlayerController` and fires the static `PlayerController.OnSwitch` event.

`SwitchModes.cs` (singleton) receives the event and runs a 0.4-second transition, broadcasting `OnTransitionUpdate(float t)` every frame where `t = 0` is fully Awake and `t = 1` is fully Asleep. All visual and gameplay systems subscribe to this rather than listening to the raw input.

| System | What it does during transition |
|---|---|
| `CameraSwitchEffect.cs` | Zooms camera in; fades in a post-processing volume |
| `SpriteController.cs` | Lerps character sprite color between two configured colors |
| `VisibilityController.cs` | Enables/disables specific colliders (platforms, doors) per state |
| `CharacterAnimatorBridge.cs` | Sets `isSleeping` bool on the animator |

**Level design rule:** any platform or collider that should exist only in one state gets a `VisibilityController` component with `visibleInAwake` set accordingly.

---

## Player Movement

Physics are raycast-based (no Rigidbody). `ObjectController2D.cs` casts rays to detect ground, walls, ceilings, and slopes, then passes a `CollisionInfo` struct to `CharacterController2D.cs`. All tunable values live in a `CharacterData` ScriptableObject assigned to the character prefab.

### Movement features

- **Walk/run** with configurable acceleration and deceleration times.
- **Variable-height jump** — releasing jump early cuts the upward velocity. Configurable max and min jump heights.
- **Extra air jumps** — configurable count.
- **Wall jump** — jump away from a wall while sliding.
- **Dash** — horizontal or omnidirectional. Gravity is disabled during the dash. Configurable distance, speed, cooldown, and number of air dashes allowed before landing.
- **Wall slide** — slow descent while touching a wall, configurable slide speed.
- **One-way platforms** — hold Down + Jump to drop through.
- **Slopes** — automatically climbs and descends slopes up to 80°. Friction is simulated on slopes.

### Key CharacterData fields

| Field | Effect |
|---|---|
| `maxSpeed` | Top horizontal speed |
| `accelerationTime` / `decelerationTime` | How quickly speed changes |
| `maxJumpHeight` / `minJumpHeight` | Jump arc bounds |
| `extraJumps` | Air jumps beyond the first |
| `dashDistance` / `dashSpeed` | Dash travel |
| `dashCooldown` | Seconds before next dash |
| `airDashesAllowed` | Dashes allowed before landing again |
| `wallSlideSpeed` | Downward speed while wall sliding |

---

## Collectibles & Interaction

### Star (required)

- 3 stars per level, must collect all 3 to unlock the exit.
- On collection: plays a puff animation, plays a sound at the camera position, destroys the object, increments `InteractSystem.CollectedStarCount`.
- HUD (`CollectableUI.cs`) shows 3 star images; each turns white as it is collected.

### Normal collectibles (optional)

- Any number per level.
- Same collection flow as stars but increment `InteractSystem.CollectedNormalCount`.
- Result screen shows `CollectedNormalCount / TotalObjects` as a completion percentage.

### Auto-collection

`InteractSystem.cs` checks within a 0.5-unit radius every frame and auto-collects the nearest `CollectibleObject` or `AnimationInteractableObject` on contact. No button press is required for collectibles.

### Pushable objects

Boxes with `PushableObject.cs` use the same raycast physics as the player. They have a `pushResistance` value that slows the player proportionally while pushing. They decelerate automatically via friction when released.

---

## Level Win Condition

`LevelManager.cs` monitors a trigger zone at the level exit. When the player enters it:

- If `CollectedStarCount >= 3` → enables `ResultUI`, showing the normal collectible count and a **Next Level** button.
- If `CollectedStarCount < 3` → does nothing (player must collect more stars).

The final level uses `FinalLevelManager.cs` instead, which disables gameplay UI, triggers the ending cutscene animator, and loads the Credits scene when the cutscene finishes.

---

## Special Jump Zones

Some levels have `SpecialJump` trigger zones. When the player enters the `SpecialStart` collider:

- The animator fires the `StarHit` trigger (special jump animation).
- `PlayerController.Jump()` is called to boost the player upward.
- `SpecialJumpEffect.cs` applies a slow-motion effect (25% time scale) and zooms the camera in by 2 units for 1.2 seconds, then restores both.

---

## Moving Platforms

`PlatformController.cs` follows a chain of `PlatformWaypoint` nodes.

- Supports configurable wait times at each waypoint and acceleration/deceleration between them.
- **Crumbling mode:** the platform disappears after the player stands on it for a set duration, then restores after a delay.
- While the player (or any object) rides the platform, their position is updated each frame to match platform movement.

---

## Triggers & Doors

`TriggerObject.cs` is the base for activatable switches. It exposes an `OnActiveChanged` event and can be set to one-shot (stays active permanently after first trigger).

`AreaTrigger.cs` activates when any object on the configured layer enters its collider (optionally player-only).

`Triggerable.cs` listens to a `TriggerObject` and mirrors its state to an Animator `active` boolean — used for doors, moving bridges, and other animated responses.

---

## Tutorial Hints

`WorldTutorialHint.cs` places text labels in world space (not screen space). The text fades out as the player moves away. An optional sprite (key icon) can appear above the text. Child `TextMeshPro` and `SpriteRenderer` objects are built at runtime from inspector fields.

---

## Audio

| Script | Sounds |
|---|---|
| `CharacterSoundController.cs` | Jump, land, poof (on switch), special jump |
| `CollectibleObject.cs` | Collection sound at camera position |
| `BackgroundMusic.cs` | Persistent BGM, switches track per scene |
| `ResultUI.cs` | Result fanfare on level complete |

All one-shot sounds use `AudioSource.PlayOneShot()`. The collection sound uses `AudioSource.PlayClipAtPoint()` at the camera position so it still plays even after the collectible object is destroyed.

---

## Key Scripts Reference

| Script | Path | Role |
|---|---|---|
| `PlayerController.cs` | 2dCharacterController/Controller | Input, jump, dash, switch input |
| `CharacterController2D.cs` | 2dCharacterController/Controller | Movement physics |
| `ObjectController2D.cs` | 2dCharacterController/Controller | Base raycast physics (shared with pushables) |
| `CharacterData.cs` | 2dCharacterController/Controller | ScriptableObject — all player tuning values |
| `InteractSystem.cs` | 2dCharacterController/Interaction | Detects and collects nearby interactables |
| `CollectibleObject.cs` | 2dCharacterController/Interaction | Star and normal collectible behaviour |
| `CollectableUI.cs` | 2dCharacterController/Interaction | HUD star display |
| `LevelManager.cs` | SceneControl | Exit trigger, win condition check |
| `FinalLevelManager.cs` | 2dCharacterController/Interaction | Final level cutscene + credits load |
| `ResultUI.cs` | 2dCharacterController/Interaction | Level complete screen |
| `SwitchModes.cs` | Switching | Central Awake/Asleep transition hub |
| `VisibilityController.cs` | Switching | Per-object state visibility |
| `CameraSwitchEffect.cs` | Switching | Camera zoom + post-processing on switch |
| `PlatformController.cs` | 2dCharacterController/Platforms | Moving and crumbling platforms |
| `BackgroundMusic.cs` | SceneControl | Persistent music, scene-aware track switching |
| `FloatingStar.cs` | UI | Decorative sine-wave bob for UI stars |
| `WorldTutorialHint.cs` | UI | In-world fade-out hint labels |
| `MobileControlsVisibility.cs` | UI | Show/hide mobile controls |

---

## Architecture Notes

- **Raycast physics, not Rigidbody.** `ObjectController2D` casts rays in each direction each frame. This gives precise control over slopes, one-way platforms, and edge cases that Unity's physics engine handles poorly in 2D platformers.
- **Event-driven state changes.** Core state transitions (switch, special jump, jump) use static C# events, so systems subscribe without direct references.
- **ScriptableObject for character data.** Swap `CharacterData` assets to change player feel per level or for testing without touching prefabs.
- **Singleton persistence.** `BackgroundMusic` and `PhysicsConfig` use `DontDestroyOnLoad` with duplicate-prevention checks.
