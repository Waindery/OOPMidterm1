# UI Setup Fix - Start Panel Issue

## Problem
- Only seeing restart button on start screen
- Buttons not working
- Can't start the game

## Quick Fix Options

### Option 1: Simple Setup (Recommended - No Panels Needed)

You don't actually need panels! You can just use buttons directly:

1. **Create Canvas** (if not exists):
   - Right-click Hierarchy → UI → Canvas

2. **Create Start Button:**
   - Right-click Canvas → UI → Button
   - Name: "StartButton"
   - Text: "Start Game"
   - Position: Center of screen

3. **Create Restart Button:**
   - Right-click Canvas → UI → Button  
   - Name: "RestartButton"
   - Text: "Restart"
   - Position: Below Start button
   - **Initially hide it** (uncheck the GameObject in Inspector)

4. **Create UI Text Elements:**
   - Right-click Canvas → UI → Text (for Score)
     - Name: "ScoreText"
     - Position: Top-Left
     - Text: "Score: 0"
   - Right-click Canvas → UI → Text (for Health)
     - Name: "HealthText"
     - Position: Top-Left, below Score
     - Text: "Health: 100 / 100"
   - Right-click Canvas → UI → Text (for Timer)
     - Name: "TimerText"
     - Position: Top-Right
     - Text: "Time: 01:00"

5. **Setup UIManager:**
   - Select UIManager GameObject
   - In Inspector, assign:
     - **Start Button**: Drag StartButton
     - **Restart Button**: Drag RestartButton
     - **Score Text**: Drag ScoreText
     - **Health Text**: Drag HealthText
     - **Timer Text**: Drag TimerText
     - **Start Panel**: Leave empty (null)
     - **Game UI Panel**: Leave empty (null)
     - **Game Over Panel**: Leave empty (null)
     - **Win Panel**: Leave empty (null)

6. **Test:**
   - Press Play
   - Click "Start Game" button
   - Game should start!

### Option 2: With Panels (Full Setup)

If you want to use panels:

1. **Create Panels:**
   - Right-click Canvas → UI → Panel
     - Name: "StartPanel"
     - Add StartButton as child of this panel
   - Right-click Canvas → UI → Panel
     - Name: "GameUIPanel"
     - Add ScoreText, HealthText, TimerText as children
   - Right-click Canvas → UI → Panel
     - Name: "GameOverPanel"
     - Add RestartButton as child
   - Right-click Canvas → UI → Panel
     - Name: "WinPanel"

2. **Setup Panel Visibility:**
   - **StartPanel**: Active (checked) initially
   - **GameUIPanel**: Inactive (unchecked) initially
   - **GameOverPanel**: Inactive (unchecked) initially
   - **WinPanel**: Inactive (unchecked) initially

3. **Assign in UIManager:**
   - Drag all panels to their respective fields
   - Drag all buttons and text elements

## Common Issues

### Buttons Not Working?

1. **Check GameManager exists:**
   - Look in Hierarchy for "GameManager" GameObject
   - If missing, create it:
     - Right-click Hierarchy → Create Empty
     - Name: "GameManager"
     - Add Component → Scripts → GameManager

2. **Check Button References:**
   - Select UIManager
   - In Inspector, make sure Start Button and Restart Button are assigned
   - They should NOT say "None (Button)"

3. **Check Console for Errors:**
   - Window → General → Console
   - Look for red error messages
   - Common error: "GameManager not found"

### Restart Button Showing on Start Screen?

**Solution:** The restart button should be hidden initially. 

1. Select RestartButton
2. In Inspector, uncheck the GameObject (makes it inactive)
3. It will be shown when game ends

OR

1. Put RestartButton inside GameOverPanel
2. GameOverPanel starts inactive
3. RestartButton will only show when panel is active

### Can't See Start Button?

1. **Check Canvas:**
   - Canvas should have "Screen Space - Overlay" render mode
   - Canvas Scaler: Scale With Screen Size

2. **Check Button Position:**
   - Button might be off-screen
   - Reset position to (0, 0, 0) in RectTransform
   - Or set Anchors to center

3. **Check Button Text:**
   - Button has a child Text object
   - Make sure Text is visible and has content

## Minimal Working Setup

**Absolute minimum to get game working:**

1. ✅ GameManager GameObject with GameManager script
2. ✅ UIManager GameObject with UIManager script  
3. ✅ Canvas with StartButton
4. ✅ StartButton assigned in UIManager
5. ✅ GameManager found by UIManager

**That's it!** The game will work with just these.

## Testing Checklist

- [ ] GameManager exists in scene
- [ ] UIManager exists in scene
- [ ] Canvas exists
- [ ] StartButton exists and is visible
- [ ] StartButton is assigned in UIManager Inspector
- [ ] GameManager has GameManager script attached
- [ ] UIManager has UIManager script attached
- [ ] Press Play → Click Start → Game starts!

## Quick Debug

Add this to test if buttons work:

1. Select StartButton
2. In Inspector, scroll to "On Click ()"
3. Click "+" to add event
4. Drag UIManager GameObject to object field
5. Select: UIManager → OnStartButtonClicked()
6. Now button will work even if GameManager is missing (for testing)

