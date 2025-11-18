# Setup Instructions for GitHub Upload

## Step 1: Install Git (if not already installed)

1. Download Git from: https://git-scm.com/download/win
2. Install with default settings
3. Restart your terminal/PowerShell after installation

## Step 2: Initialize Git Repository

Open PowerShell or Command Prompt in your project folder (`C:\Users\COMPUTER\Desktop\OOPMidterm1`) and run:

```bash
git init
git add .
git commit -m "Initial commit: Unity OOP Game Project"
```

## Step 3: Create GitHub Repository

1. Go to https://github.com
2. Sign in to your account (or create one if needed)
3. Click the "+" icon in the top right corner
4. Select "New repository"
5. Repository name: `OOPMidterm1` (or any name you prefer)
6. Description: "Unity game demonstrating OOP concepts"
7. Set visibility to **Public** (required for assignment)
8. **DO NOT** initialize with README, .gitignore, or license (we already have these)
9. Click "Create repository"

## Step 4: Connect Local Repository to GitHub

After creating the repository, GitHub will show you commands. Use these (replace YOUR_USERNAME with your GitHub username):

```bash
git remote add origin https://github.com/YOUR_USERNAME/OOPMidterm1.git
git branch -M main
git push -u origin main
```

If you get authentication errors, you may need to:
- Use a Personal Access Token instead of password
- Or use GitHub Desktop application (easier option)

## Alternative: Using GitHub Desktop (Easier Method)

1. Download GitHub Desktop: https://desktop.github.com/
2. Install and sign in with your GitHub account
3. Click "File" → "Add Local Repository"
4. Browse to: `C:\Users\COMPUTER\Desktop\OOPMidterm1`
5. Click "Publish repository"
6. Make sure "Keep this code private" is **UNCHECKED** (to make it public)
7. Click "Publish repository"

## Step 5: Get Your Repository Link

After uploading, your repository link will be:
```
https://github.com/YOUR_USERNAME/OOPMidterm1
```

Copy this link and submit it for your assignment.

## Step 6: Unity Setup (Before Testing)

Before testing the game, you need to set up the Unity scene:

### A. Create Game Objects

1. **Ground:**
   - Create → 3D Object → Plane
   - Scale: (10, 1, 10)
   - Position: (0, 0, 0)

2. **Player:**
   - Create → 3D Object → Cube
   - Name: "Player"
   - Position: (0, 0.5, 0)
   - Tag: "Player" (create tag if needed)
   - Add Component → `Player.cs` script
   - Add Component → Rigidbody
   - Add Component → Box Collider (check "Is Trigger")

3. **GameManager:**
   - Create → Empty GameObject
   - Name: "GameManager"
   - Add Component → `GameManager.cs` script

4. **UIManager:**
   - Create → Empty GameObject
   - Name: "UIManager"
   - Add Component → `UIManager.cs` script

### B. Create Prefabs

1. **Enemy Prefab:**
   - Create → 3D Object → Sphere
   - Name: "Enemy"
   - Position: (0, 0.5, 0)
   - Add Component → `Enemy.cs` script
   - Add Component → Rigidbody (freeze rotation X, Y, Z)
   - Add Component → Sphere Collider (check "Is Trigger")
   - Drag to Project window to create prefab
   - Delete from scene

2. **Item Prefab:**
   - Create → 3D Object → Cube
   - Name: "Item"
   - Position: (0, 0.5, 0)
   - Scale: (0.5, 0.5, 0.5)
   - Add Component → `Item.cs` script
   - Add Component → Box Collider (check "Is Trigger")
   - Drag to Project window to create prefab
   - Delete from scene

### C. Setup UI

1. **Create Canvas:**
   - Right-click in Hierarchy → UI → Canvas

2. **Create UI Elements:**
   - Right-click Canvas → UI → Text (for Score)
     - Name: "ScoreText"
     - Position: Top-Left
   - Right-click Canvas → UI → Text (for Health)
     - Name: "HealthText"
     - Position: Top-Left, below Score
   - Right-click Canvas → UI → Text (for Timer)
     - Name: "TimerText"
     - Position: Top-Right
   - Right-click Canvas → UI → Button (for Start)
     - Name: "StartButton"
     - Text: "Start Game"
   - Right-click Canvas → UI → Button (for Restart)
     - Name: "RestartButton"
     - Text: "Restart"
   - Right-click Canvas → UI → Panel (for Start Screen)
     - Name: "StartPanel"
   - Right-click Canvas → UI → Panel (for Game Over Screen)
     - Name: "GameOverPanel"
   - Right-click Canvas → UI → Panel (for Win Screen)
     - Name: "WinPanel"
   - Right-click Canvas → UI → Panel (for Game UI)
     - Name: "GameUIPanel"

3. **Assign References in UIManager:**
   - Select UIManager GameObject
   - In Inspector, drag all UI elements to their corresponding fields in UIManager script

4. **Assign Prefabs in GameManager:**
   - Select GameManager GameObject
   - In Inspector, drag Enemy prefab to "Enemy Prefab" field
   - Drag Item prefab to "Item Prefab" field

### D. Test the Game

1. Press Play in Unity
2. Click "Start Game" button
3. Use WASD to move
4. Press Space to attack
5. Collect items and avoid enemies!

## Troubleshooting

- **Scripts not showing:** Make sure scripts are in `Assets/Scripts/` folder
- **Null reference errors:** Make sure all references are assigned in Inspector
- **Player not moving:** Check that Player has Rigidbody and is tagged "Player"
- **Items not collecting:** Make sure colliders are set as Triggers
- **UI not updating:** Check that UIManager references are assigned

## Files Created

✅ All 5 C# scripts in `Assets/Scripts/`
✅ README.md with full documentation
✅ .gitignore for Unity
✅ This setup guide

You're ready to upload to GitHub!

