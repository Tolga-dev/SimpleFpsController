# Simple FPS Player Controller for Unity
it is a simple player controller for unity. It includes basic movement options, gun mechanics, and item interactions.
## Features

### Player States
- **Movement**
  - Walk
  - Run
  - Crouch
    - Fast Crouch
  - Prone
    - Fast Prone
  - Slide
  - Jump
  - Leaning (Left/Right)
  - Climbing
  - Idle
- **Drinkable Items**
  - Booster
- **Health Items**
  - Health Pack
  - Small Health Pack

### Gun States
- Gun Mode Switching

### Input Bindings
- **Jump:** `Space`
- **Run:** `LeftShift`
- **Crouch:** `C`
- **Slide:** `LeftControl`
- **Prone:** `Z`
- **Lean Left:** `Q`
- **Lean Right:** `E`

### Input Variables
```csharp
public bool isJumping;
public bool isRunning;
public bool isCrouching;
public bool isSliding;
public bool isProne;
public bool isLeaningLeft;
public bool isLeaningRight;
public bool isClimbing;
public bool isIdle;

public float mouseX;
public float mouseY;

public float x;
public float z;

public KeyCode jumpKey = KeyCode.Space;
public KeyCode runKey = KeyCode.LeftShift;
public KeyCode crouchKey = KeyCode.C;
public KeyCode slideKey = KeyCode.LeftControl;
public KeyCode proneKey = KeyCode.Z;
public KeyCode leanLeftKey = KeyCode.Q;
public KeyCode leanRightKey = KeyCode.E;
```
# Download
download and add **simpleFps.unitypackage** into your project

# Usage
Add this controller to your FPS player character in Unity to enable diverse movement options,
gun mechanics, and item interactions.

# Licence
Free to use

