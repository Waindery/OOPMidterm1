# Debugging Guide - Buttons Not Working

## Problem
- Clicking buttons does nothing
- Buttons stay visible after clicking
- Game doesn't start

## Step-by-Step Debugging

### Step 1: Check Console for Messages

1. **Open Console:**
   - Window → General → Console
   - Or press Ctrl+Shift+C

2. **Press Play in Unity**

3. **Click the Start or Restart button**

4. **Look for messages in Console:**
   - ✅ "Start Button Clicked!" or "Restart Button Clicked!" = Button is working!
   - ✅ "GameManager found! Starting game..." = Everything is connected!
   - ❌ "GameManager not found!" = GameManager is missing (see Step 2)
   - ❌ No messages at all = Button not connected (see Step 3)

### Step 2: Create GameManager (If Missing)

If you see "GameManager not found!":

1. **Create GameManager:**
   - Right-click Hierarchy → Create Empty
   - Name: "GameManager"

2. **Add Script:**
   - Select GameManager
   - Add Component → Scripts → GameManager

3. **Test again:**
   - Press Play
   - Click button
   - Should see "GameManager found!" in console

### Step 3: Connect Buttons Manually (If No Messages)

If clicking button shows NO messages in console:

1. **Select the Button** (StartButton or RestartButton) in Hierarchy

2. **In Inspector, scroll to Button component**

3. **Find "On Click ()" section**

4. **Check if there's an event:**
   - If empty: Click "+" to add event
   - If has event: Check if it's connected correctly

5. **Connect the button:**
   - Click "+" to add event
   - Drag **UIManager GameObject** from Hierarchy to the object field
   - In dropdown, select:
     - For StartButton: `UIManager → OnStartButtonClicked()`
     - For RestartButton: `UIManager → OnRestartButtonClicked()`

6. **Test:**
   - Press Play
   - Click button
   - Should see "Button Clicked!" in console

### Step 4: Check UIManager Setup

1. **Select UIManager** in Hierarchy

2. **In Inspector, check:**
   - UIManager script is attached ✅
   - Start Button is assigned (not "None") ✅
   - Restart Button is assigned (not "None") ✅

3. **If buttons are "None":**
   - Drag buttons from Hierarchy to the fields

### Step 5: Verify Game Starts

After clicking button, check:

1. **Console shows:**
   - "Start Button Clicked!"
   - "GameManager found! Starting game..."
   - "Game Started!"

2. **Buttons should disappear** (hidden when game starts)

3. **Game UI should appear:**
   - Score text visible
   - Health text visible
   - Timer text visible

4. **Player should be controllable:**
   - Use WASD to move
   - Player should move around

## Common Issues & Fixes

### Issue: "GameManager not found!"
**Fix:** Create GameManager GameObject with GameManager script

### Issue: No console messages when clicking
**Fix:** Button not connected - manually connect in Inspector (Step 3)

### Issue: Buttons don't disappear
**Fix:** 
- Check if buttons are children of a panel
- Make sure ShowStartScreen(false) is being called
- Check console for errors

### Issue: Game starts but nothing happens
**Fix:**
- Check if Player exists and has Player script
- Check if Enemy/Item prefabs are assigned in GameManager
- Check console for spawn errors

### Issue: Can't see game (black screen)
**Fix:**
- Check camera position
- Make sure camera is enabled
- Check if player is on the plane

## Quick Test Checklist

- [ ] Console window is open
- [ ] GameManager exists in scene
- [ ] UIManager exists in scene
- [ ] Buttons are assigned in UIManager
- [ ] Buttons are connected (manually or automatically)
- [ ] Press Play
- [ ] Click button
- [ ] See messages in console
- [ ] Buttons disappear
- [ ] Game UI appears
- [ ] Can move player with WASD

## What Should Happen

**When you click Start/Restart button:**

1. Console shows: "Start Button Clicked!" or "Restart Button Clicked!"
2. Console shows: "GameManager found! Starting game..."
3. Console shows: "Game Started!"
4. Buttons disappear
5. Score/Health/Timer text appears
6. Player can move with WASD
7. Enemies and items spawn after a few seconds

If any step fails, check the console for error messages!

