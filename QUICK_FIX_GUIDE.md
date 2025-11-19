# Quick Fix Guide - Player Falling Through Plane

## Problem
- Player cube falls through the plane
- Nothing visible on screen when playing

## Solution Steps

### Step 1: Fix the Plane (Ground)

1. **Select the Plane** in Hierarchy
2. **Check Inspector:**
   - Position: (0, 0, 0)
   - Rotation: (0, 0, 0)
   - Scale: (1, 1, 1) or larger like (10, 1, 10)
3. **Add Box Collider** (if not present):
   - Component → Physics → Box Collider
   - Make sure it's enabled
   - Size should match the plane

### Step 2: Fix the Player

1. **Select the Player Cube** in Hierarchy
2. **Check Position:**
   - Position: (0, 1, 0) or (0, 0.5, 0) - **MUST be ABOVE the plane**
   - The plane is at Y=0, so player should be at Y=0.5 or higher
3. **Check Components:**
   - **Player.cs script** - Should be attached
   - **Rigidbody** - Should be present
     - Use Gravity: ✅ Checked (enabled)
     - Freeze Rotation: ✅ Check X, Y, Z
   - **Box Collider** - Should be present
     - Is Trigger: ✅ Checked (for item/enemy detection)
     - Size: (1, 1, 1) or adjust as needed
4. **Check Tag:**
   - Tag: "Player" (create tag if it doesn't exist)

### Step 3: Fix the Camera

1. **Select Main Camera** in Hierarchy
2. **Set Position:**
   - Position: (0, 5, -10)
   - Rotation: (15, 0, 0) or (25, 0, 0)
3. **This will give you a top-down or angled view**

### Step 4: Create GameManager

1. **Create Empty GameObject:**
   - Right-click Hierarchy → Create Empty
   - Name: "GameManager"
2. **Add Script:**
   - Add Component → Scripts → GameManager
3. **In Inspector, assign:**
   - Enemy Prefab: (create this next)
   - Item Prefab: (create this next)

### Step 5: Create Enemy Prefab

1. **Create Sphere:**
   - Right-click Hierarchy → 3D Object → Sphere
   - Name: "Enemy"
   - Position: (5, 0.5, 5) (somewhere visible)
2. **Add Components:**
   - Add Component → Scripts → Enemy
   - Add Component → Physics → Rigidbody
     - Use Gravity: ✅ Checked
     - Freeze Rotation: ✅ Check X, Y, Z
   - Add Component → Physics → Sphere Collider
     - Is Trigger: ✅ Checked
3. **Create Prefab:**
   - Drag "Enemy" from Hierarchy to Project window (create Prefabs folder first)
   - Delete "Enemy" from scene (keep the prefab)

### Step 6: Create Item Prefab

1. **Create Cube:**
   - Right-click Hierarchy → 3D Object → Cube
   - Name: "Item"
   - Position: (-5, 0.5, -5)
   - Scale: (0.5, 0.5, 0.5) - make it smaller
2. **Add Components:**
   - Add Component → Scripts → Item
   - Add Component → Physics → Box Collider
     - Is Trigger: ✅ Checked
3. **Create Prefab:**
   - Drag "Item" from Hierarchy to Project window
   - Delete "Item" from scene (keep the prefab)

### Step 7: Assign Prefabs to GameManager

1. **Select GameManager** in Hierarchy
2. **In Inspector:**
   - Drag Enemy prefab to "Enemy Prefab" field
   - Drag Item prefab to "Item Prefab" field

### Step 8: Basic UI Setup (Minimum)

1. **Create Canvas:**
   - Right-click Hierarchy → UI → Canvas
2. **Create Text for Score:**
   - Right-click Canvas → UI → Text
   - Name: "ScoreText"
   - Position: Top-Left
   - Text: "Score: 0"
3. **Create Start Button:**
   - Right-click Canvas → UI → Button
   - Name: "StartButton"
   - Text: "Start Game"
   - Position: Center

### Step 9: Create UIManager

1. **Create Empty GameObject:**
   - Right-click Hierarchy → Create Empty
   - Name: "UIManager"
2. **Add Script:**
   - Add Component → Scripts → UIManager
3. **Assign References (minimum):**
   - Drag ScoreText to "Score Text" field
   - Drag StartButton to "Start Button" field

### Step 10: Test

1. **Press Play**
2. **You should see:**
   - Player cube on the plane
   - Camera showing the scene
   - UI with Start button
3. **Click Start Button**
4. **Use WASD to move** - player should move and stay on plane
5. **Enemies and items should spawn** after clicking Start

## Common Issues

### Player still falling?
- Check player Y position is above 0
- Check plane has Box Collider
- Check Rigidbody is on player (not kinematic)

### Can't see anything?
- Check camera position and rotation
- Check camera is enabled
- Try moving camera to (0, 10, 0) with rotation (90, 0, 0) for top-down view

### Game not starting?
- Make sure GameManager exists
- Make sure UIManager exists
- Make sure Start button is connected to UIManager

### Items/Enemies not spawning?
- Check prefabs are assigned in GameManager
- Check GameManager script is enabled
- Check console for errors (Window → General → Console)

## Quick Camera Setup (Top-Down View)

If you want a simple top-down view:

1. Select Main Camera
2. Position: (0, 10, 0)
3. Rotation: (90, 0, 0)
4. This gives you a bird's-eye view

## Quick Camera Setup (Angled View)

For a better 3D view:

1. Select Main Camera
2. Position: (0, 8, -8)
3. Rotation: (35, 0, 0)
4. This gives you an angled view

