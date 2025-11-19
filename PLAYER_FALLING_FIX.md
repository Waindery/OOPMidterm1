# Fix: Player Falling Through Plane

## Quick Fix Steps

### Step 1: Check Plane (Ground)

1. **Select the Plane** in Hierarchy
2. **In Inspector, check:**
   - ✅ Has **Box Collider** component
   - ✅ Box Collider is **Enabled** (checkbox checked)
   - ✅ Position: Y = 0
   - ✅ Scale: At least (1, 1, 1) or larger like (10, 1, 10)

3. **If no Box Collider:**
   - Add Component → Physics → Box Collider
   - Make sure it's enabled

### Step 2: Fix Player Position

1. **Select Player Cube** in Hierarchy
2. **Set Position:**
   - X: 0
   - Y: 1 (or 0.5) ← **MUST be above 0!**
   - Z: 0
3. **The plane is at Y=0, so player must be at Y > 0**

### Step 3: Fix Player Rigidbody

1. **Select Player** in Hierarchy
2. **Check Rigidbody component:**
   - ✅ **Use Gravity**: Checked (ON)
   - ✅ **Freeze Rotation**: X, Y, Z all checked
   - ✅ **Is Kinematic**: Unchecked (OFF)

3. **If Rigidbody settings are wrong, fix them:**
   - Use Gravity: ✅ ON
   - Freeze Rotation: ✅ X, Y, Z all ON
   - Is Kinematic: ❌ OFF

### Step 4: Check Player Collider

1. **Select Player**
2. **Check Box Collider:**
   - ✅ Box Collider exists
   - ✅ Is Trigger: Can be ON or OFF (both work)
   - ✅ Size should be reasonable (1, 1, 1)

### Step 5: Test

1. **Press Play**
2. **Player should stay on plane**
3. **Use WASD to move** - player should move and stay on ground

## Alternative: Use Kinematic Rigidbody (No Gravity)

If player still falls, try this:

1. **Select Player**
2. **In Rigidbody component:**
   - **Is Kinematic**: ✅ Checked (ON)
   - **Use Gravity**: ❌ Unchecked (OFF)
3. **This disables physics but keeps collision detection**

## Common Issues

### Player falls immediately
- **Fix:** Player Y position is 0 or negative
- **Solution:** Set Player Y position to 1 or 0.5

### Player falls slowly
- **Fix:** Plane has no collider
- **Solution:** Add Box Collider to Plane

### Player bounces or jitters
- **Fix:** Rigidbody settings wrong
- **Solution:** Freeze Rotation X, Y, Z

### Player moves but still falls
- **Fix:** Plane collider might be too small
- **Solution:** Increase Plane scale or check collider size

## Step-by-Step Complete Setup

### 1. Create/Fix Plane

```
1. Create → 3D Object → Plane (or select existing)
2. Position: (0, 0, 0)
3. Scale: (10, 1, 10) - makes it bigger
4. Add Component → Physics → Box Collider (if missing)
5. Make sure Box Collider is enabled
```

### 2. Create/Fix Player

```
1. Create → 3D Object → Cube
2. Name: "Player"
3. Position: (0, 1, 0) ← Important: Y = 1!
4. Tag: "Player" (create tag if needed)
5. Add Component → Scripts → Player
6. Add Component → Physics → Rigidbody
   - Use Gravity: ON
   - Freeze Rotation: X, Y, Z all ON
   - Is Kinematic: OFF
7. Add Component → Physics → Box Collider
   - Is Trigger: ON (for item/enemy detection)
```

### 3. Test

Press Play - player should stay on plane!

## Still Not Working?

Try this test:

1. **Create a simple test:**
   - Create → 3D Object → Cube
   - Position: (0, 1, 0)
   - Add Component → Physics → Rigidbody
   - Use Gravity: ON
   - Freeze Rotation: X, Y, Z
   - Press Play
   - Does this cube fall? If yes, plane collider is the problem

2. **Check plane collider:**
   - Select Plane
   - In Box Collider, check "Size"
   - Should be (10, 0.1, 10) or similar
   - If size is (0, 0, 0), that's the problem!

## Quick Checklist

- [ ] Plane has Box Collider (enabled)
- [ ] Player Y position > 0 (like 1 or 0.5)
- [ ] Player has Rigidbody
- [ ] Rigidbody: Use Gravity = ON
- [ ] Rigidbody: Freeze Rotation = X, Y, Z all ON
- [ ] Rigidbody: Is Kinematic = OFF
- [ ] Press Play → Player stays on plane

Try these steps and let me know which step fixes it!

