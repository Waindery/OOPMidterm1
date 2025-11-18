# Unity OOP Game Project

A complete Unity game demonstrating Object-Oriented Programming (OOP) concepts including class structure, inheritance, encapsulation, method overloading, and class interactions.

## Game Overview

**Game Type:** Collect Items and Avoid Enemies

**Objective:** Collect items to score points while avoiding enemies. Reach the target score before time runs out or your health reaches zero.

## OOP Concepts Demonstrated

### 1. Class Structure

The project includes 5 main classes, each demonstrating core OOP principles:

#### **GameManager.cs**
- **Purpose:** Controls game flow, timer, spawning, and win/lose logic
- **Fields (10+):** gameTimer, maxGameTime, currentScore, targetScore, isGameActive, spawnInterval, maxEnemies, maxItems, difficultyLevel, enemySpeedMultiplier, etc.
- **Methods (15+):** 
  - `StartGame()` - Starts the game
  - `AddScore(int points)` - Adds score (with parameters)
  - `AddScore(int points, float multiplier)` - Overloaded method
  - `GetDifficulty()` - Returns difficulty level (return value)
  - `CalculateScoreMultiplier()` - Returns multiplier (return value)
  - `SetDifficulty(int level)` - Sets difficulty (with parameters)
  - And more...

#### **Player.cs**
- **Purpose:** Handles player movement, actions, health, and interactions
- **Fields (15+):** moveSpeed, rotationSpeed, currentVelocity, maxHealth, currentHealth, invincibilityDuration, isInvincible, attackDamage, attackRange, playerName, itemsCollected, etc.
- **Methods (15+):**
  - `TakeDamage(int damage)` - Takes damage (with parameters)
  - `Heal(int healAmount)` - Heals player (with parameters)
  - `Attack()` - Basic attack
  - `Attack(int damage)` - Overloaded attack method
  - `GetHealth()` - Returns health (return value)
  - `GetHealthPercentage()` - Returns percentage (return value)
  - `CollectItem(Item item)` - Interacts with Item class
  - And more...

#### **Enemy.cs**
- **Purpose:** Handles enemy AI behavior, movement, health, and interactions
- **Fields (15+):** moveSpeed, rotationSpeed, detectionRange, maxHealth, currentHealth, damage, isChasing, enemyType, scoreValue, isAlive, etc.
- **Methods (15+):**
  - `TakeDamage(int damageAmount)` - Takes damage (with parameters)
  - `TakeDamage(int damageAmount, float knockbackForce)` - Overloaded method
  - `SetSpeedMultiplier(float multiplier)` - Sets speed (with parameters)
  - `GetHealth()` - Returns health (return value)
  - `GetDistanceToPlayer()` - Returns distance (return value)
  - `AttackPlayer()` - Interacts with Player class
  - And more...

#### **Item.cs**
- **Purpose:** Handles collectible items that can be picked up by the player
- **Fields (15+):** itemType, value, itemName, itemColor, rotationSpeed, floatSpeed, floatAmplitude, isCollected, healthRestore, scorePoints, spawnTime, lifetime, etc.
- **Methods (15+):**
  - `SetItemType(string type)` - Sets type (with parameters)
  - `SetValue(int newValue)` - Sets value (with parameters)
  - `SetValue(string type, int newValue)` - Overloaded method
  - `GetValue()` - Returns value (return value)
  - `GetItemType()` - Returns type (return value)
  - `GetRemainingLifetime()` - Returns lifetime (return value)
  - And more...

#### **UIManager.cs**
- **Purpose:** Handles all UI updates and interactions
- **Fields (15+):** scoreText, healthText, timerText, gameOverText, winText, gameUIPanel, startPanel, gameOverPanel, winPanel, scorePrefix, healthPrefix, currentScoreDisplay, etc.
- **Methods (15+):**
  - `UpdateScore(int score)` - Updates score (with parameters)
  - `UpdateScore(int score, string prefix)` - Overloaded method
  - `UpdateHealth(int health, int maxHealth)` - Updates health (with parameters)
  - `UpdateTimer(float currentTime, float maxTime)` - Updates timer (with parameters)
  - `GetCurrentScore()` - Returns score (return value)
  - `CalculateHealthPercentage(int health, int maxHealth)` - Returns percentage (return value)
  - And more...

### 2. Class Interactions

Classes communicate through method calls, parameters, and return values:

- **Player → Item:** `player.CollectItem(item)` - Player collects item
- **Player → Enemy:** `player.Attack(enemy)` - Player attacks enemy
- **Enemy → Player:** `enemy.AttackPlayer()` - Enemy damages player
- **GameManager → Player:** `gameManager.AddScore(points)` - Updates score
- **GameManager → UIManager:** `uiManager.UpdateScore(score)` - Updates UI
- **Player → UIManager:** `uiManager.UpdateHealth(health, maxHealth)` - Updates health display
- **GameManager → Enemy:** `enemy.SetSpeedMultiplier(multiplier)` - Sets enemy speed

### 3. C# Language Features Used

- **Conditions:** if/else statements throughout all classes
- **Loops:** foreach loops in GameManager for managing lists
- **Variables:** Multiple variable types (int, float, string, bool, Vector3, Color, etc.)
- **Arrays/Lists:** `List<GameObject>` for activeEnemies and activeItems
- **Math Library:** `Mathf.Clamp()`, `Mathf.Max()`, `Mathf.Min()`, `Mathf.RoundToInt()`, `Vector3.Distance()`
- **Random Library:** `Random.Range()` for spawning and item generation
- **Debugging:** `Debug.Log()` statements throughout
- **UI Methods:** Text updates, button listeners, panel management

### 4. Unity Integration

- **Start():** Used in all classes for initialization
- **Update():** Used in all classes for continuous updates
- **OnTriggerEnter():** Used in Player, Enemy, and Item for collision detection
- **OnCollisionEnter():** Used in Player and Enemy for collision handling

### 5. Game Features

#### Core Gameplay Loop:
1. **Start Condition:** Press Start button to begin
2. **Loop Condition:** Collect items, avoid enemies, manage health and time
3. **End Condition:** Win by reaching target score, or lose by running out of time/health

#### Additional Features:
- **Difficulty Levels:** 3 difficulty levels with adjustable enemy speed and spawn rates
- **Score Multiplier:** Time-based score multiplier system
- **UI System:** Complete UI with score, health, timer, and game state screens
- **Prefab System:** Enemy and Item prefabs for spawning
- **Round System:** Score-based progression with increasing difficulty

## Setup Instructions

### Prerequisites
- Unity 2020.3 or later
- Basic knowledge of Unity Editor

### Installation Steps

1. **Clone or Download the Repository**
   ```bash
   git clone [repository-url]
   ```

2. **Open in Unity**
   - Open Unity Hub
   - Click "Add" and select the project folder
   - Open the project

3. **Scene Setup**
   - Open `Assets/Scenes/SampleScene.unity`
   - The scene should already be set up, but if not:
     - Create a Plane for the ground
     - Create a Cube for the Player (tag it "Player")
     - Create a Sphere for Enemy prefab
     - Create a Cube for Item prefab
     - Set up UI Canvas with Text elements and Buttons

4. **Prefab Setup**
   - Create Enemy prefab:
     - Add `Enemy.cs` script
     - Add Collider (set as Trigger)
     - Add Rigidbody (freeze rotation)
   - Create Item prefab:
     - Add `Item.cs` script
     - Add Collider (set as Trigger)
     - Add Renderer component

5. **GameObject Setup**
   - Create empty GameObject named "GameManager"
     - Add `GameManager.cs` script
     - Assign Enemy and Item prefabs in inspector
   - Create empty GameObject named "UIManager"
     - Add `UIManager.cs` script
     - Assign all UI Text and Button references in inspector
   - Setup Player GameObject:
     - Add `Player.cs` script
     - Add Rigidbody component
     - Tag as "Player"
     - Add Collider

6. **UI Setup**
   - Create Canvas
   - Add Text elements for Score, Health, Timer
   - Add Buttons for Start, Restart, Difficulty
   - Assign all references in UIManager inspector

7. **Play the Game**
   - Press Play in Unity Editor
   - Click Start button
   - Use WASD or Arrow Keys to move
   - Press Space to attack enemies
   - Collect items to gain score and health

## Project Structure

```
Assets/
├── Scripts/
│   ├── GameManager.cs      # Main game controller
│   ├── Player.cs           # Player controller
│   ├── Enemy.cs            # Enemy AI
│   ├── Item.cs             # Collectible items
│   └── UIManager.cs        # UI controller
└── Scenes/
    └── SampleScene.unity   # Main game scene
```

## Code Quality Features

- **Naming Conventions:** Clear, descriptive names following C# conventions
- **Comments:** Comprehensive XML documentation comments
- **Structure:** Well-organized classes with logical grouping
- **Encapsulation:** Private fields with public methods
- **Method Overloading:** Multiple examples throughout classes
- **Error Handling:** Null checks and validation

## Requirements Checklist

✅ At least 4 different classes (5 classes total)
✅ Each class has minimum 5 fields (all have 10+)
✅ Each class has minimum 5 methods (all have 15+)
✅ Methods with parameters (all classes)
✅ Methods with return values (all classes)
✅ Overloaded methods (GameManager, Player, Enemy, Item, UIManager)
✅ Class interactions through methods
✅ Start(), Update(), OnTriggerEnter(), OnCollisionEnter() used
✅ At least 2 prefabs (Enemy, Item)
✅ UI elements (Score, Health, Timer)
✅ Complete game loop (Start, Loop, End conditions)
✅ Difficulty levels
✅ Object arrays/lists for spawning
✅ Score multiplier system
✅ UI buttons for Start/Restart

## Game Controls

- **WASD / Arrow Keys:** Move player
- **Space:** Attack nearby enemies
- **Mouse:** Not used (can be added for camera control)

## Game Rules

- Collect items to gain score and health
- Avoid enemies or attack them with Space
- Reach target score to win
- Don't let health reach 0
- Don't let timer reach 0
- Difficulty affects enemy speed and spawn rate

## Future Enhancements

Potential additions:
- Power-up items
- Multiple enemy types
- Sound effects and music
- Particle effects
- Save/load system
- High score tracking

## License

This project is created for educational purposes to demonstrate OOP concepts in Unity.

## Author

Created for OOP Midterm Project demonstrating Object-Oriented Programming principles.

