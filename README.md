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
- **Scene Builder:** Automatic arena + lighting setup for consistent testing
- **Round System:** Score-based progression with increasing difficulty