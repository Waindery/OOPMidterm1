using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameManager class controls the overall game flow, timer, spawning, and win/lose logic.
/// Demonstrates OOP concepts: encapsulation, method parameters, return values, and class interactions.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Fields (minimum 5 required)
    [Header("Game Settings")]
    public float gameTimer;              // Current game time
    public float maxGameTime;            // Maximum time for the game
    public int currentScore;              // Player's current score
    public int targetScore;               // Score needed to win
    public bool isGameActive;             // Whether the game is currently running
    
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;        // Prefab for enemy objects
    public GameObject itemPrefab;         // Prefab for item objects
    public float spawnInterval;           // Time between spawns
    public int maxEnemies;                // Maximum enemies on screen
    public int maxItems;                  // Maximum items on screen
    
    [Header("Difficulty Settings")]
    public int difficultyLevel;           // Current difficulty (1-3)
    public float enemySpeedMultiplier;    // Speed multiplier for enemies
    public float spawnRateMultiplier;     // Spawn rate multiplier
    
    // Private fields
    private float spawnTimer;             // Timer for spawning
    private List<GameObject> activeEnemies; // List of active enemies
    private List<GameObject> activeItems;   // List of active items
    private UIManager uiManager;           // Reference to UI Manager
    private Player player;                 // Reference to Player
    
    /// <summary>
    /// Unity Start method - initializes the game
    /// </summary>
    void Start()
    {
        InitializeGame();
    }
    
    /// <summary>
    /// Unity Update method - handles game loop
    /// </summary>
    void Update()
    {
        if (isGameActive)
        {
            UpdateGameTimer();
            HandleSpawning();
            CheckWinCondition();
            CheckLoseCondition();
        }
    }
    
    /// <summary>
    /// Initializes the game with default values
    /// </summary>
    public void InitializeGame()
    {
        gameTimer = 0f;
        maxGameTime = 60f;
        currentScore = 0;
        targetScore = 10;
        isGameActive = false;
        difficultyLevel = 1;
        enemySpeedMultiplier = 1f;
        spawnRateMultiplier = 1f;
        spawnInterval = 3f;
        maxEnemies = 5;
        maxItems = 8;
        
        spawnTimer = 0f;
        activeEnemies = new List<GameObject>();
        activeItems = new List<GameObject>();
        
        // Find references
        uiManager = FindObjectOfType<UIManager>();
        player = FindObjectOfType<Player>();
        
        if (uiManager != null)
        {
            uiManager.UpdateScore(currentScore);
            uiManager.UpdateTimer(gameTimer, maxGameTime);
        }
        
        Debug.Log("Game initialized. Press Start to begin!");
    }
    
    /// <summary>
    /// Starts the game
    /// </summary>
    public void StartGame()
    {
        isGameActive = true;
        gameTimer = 0f;
        currentScore = 0;
        
        // Clear existing objects
        ClearAllObjects();
        
        // Update UI
        if (uiManager != null)
        {
            uiManager.ShowGameUI(true);
            uiManager.UpdateScore(currentScore);
        }
        
        Debug.Log("Game Started!");
    }
    
    /// <summary>
    /// Stops the game
    /// </summary>
    public void StopGame()
    {
        isGameActive = false;
        ClearAllObjects();
        
        if (uiManager != null)
        {
            uiManager.ShowGameUI(false);
        }
    }
    
    /// <summary>
    /// Restarts the game
    /// </summary>
    public void RestartGame()
    {
        StopGame();
        InitializeGame();
        StartGame();
    }
    
    /// <summary>
    /// Updates the game timer
    /// </summary>
    private void UpdateGameTimer()
    {
        gameTimer += Time.deltaTime;
        
        if (uiManager != null)
        {
            uiManager.UpdateTimer(gameTimer, maxGameTime);
        }
    }
    
    /// <summary>
    /// Handles spawning of enemies and items
    /// </summary>
    private void HandleSpawning()
    {
        spawnTimer += Time.deltaTime;
        float adjustedSpawnInterval = spawnInterval / spawnRateMultiplier;
        
        if (spawnTimer >= adjustedSpawnInterval)
        {
            spawnTimer = 0f;
            
            // Spawn enemy if under limit
            if (activeEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }
            
            // Spawn item if under limit
            if (activeItems.Count < maxItems)
            {
                SpawnItem();
            }
        }
    }
    
    /// <summary>
    /// Spawns an enemy at a random position
    /// </summary>
    private void SpawnEnemy()
    {
        if (enemyPrefab == null) return;
        
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        
        // Set enemy speed based on difficulty
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.SetSpeedMultiplier(enemySpeedMultiplier);
        }
        
        activeEnemies.Add(enemy);
    }
    
    /// <summary>
    /// Spawns an item at a random position
    /// </summary>
    private void SpawnItem()
    {
        if (itemPrefab == null) return;
        
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject item = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        activeItems.Add(item);
    }
    
    /// <summary>
    /// Gets a random spawn position on the map
    /// </summary>
    /// <returns>Random Vector3 position</returns>
    private Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(-8f, 8f);
        float z = Random.Range(-8f, 8f);
        return new Vector3(x, 0.5f, z);
    }
    
    /// <summary>
    /// Adds score to the player (method with parameters)
    /// </summary>
    /// <param name="points">Points to add to score</param>
    public void AddScore(int points)
    {
        currentScore += points;
        
        if (uiManager != null)
        {
            uiManager.UpdateScore(currentScore);
        }
        
        Debug.Log($"Score added: {points}. Total: {currentScore}");
    }
    
    /// <summary>
    /// Overloaded method: Adds score with multiplier
    /// </summary>
    /// <param name="points">Base points</param>
    /// <param name="multiplier">Score multiplier</param>
    public void AddScore(int points, float multiplier)
    {
        int finalPoints = Mathf.RoundToInt(points * multiplier);
        AddScore(finalPoints);
    }
    
    /// <summary>
    /// Removes an enemy from the active list
    /// </summary>
    /// <param name="enemy">Enemy GameObject to remove</param>
    public void RemoveEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }
    
    /// <summary>
    /// Removes an item from the active list
    /// </summary>
    /// <param name="item">Item GameObject to remove</param>
    public void RemoveItem(GameObject item)
    {
        if (activeItems.Contains(item))
        {
            activeItems.Remove(item);
        }
    }
    
    /// <summary>
    /// Sets the difficulty level (method with parameters)
    /// </summary>
    /// <param name="level">Difficulty level (1-3)</param>
    public void SetDifficulty(int level)
    {
        difficultyLevel = Mathf.Clamp(level, 1, 3);
        
        // Adjust game parameters based on difficulty
        switch (difficultyLevel)
        {
            case 1:
                enemySpeedMultiplier = 1f;
                spawnRateMultiplier = 1f;
                targetScore = 10;
                break;
            case 2:
                enemySpeedMultiplier = 1.5f;
                spawnRateMultiplier = 1.3f;
                targetScore = 15;
                break;
            case 3:
                enemySpeedMultiplier = 2f;
                spawnRateMultiplier = 1.6f;
                targetScore = 20;
                break;
        }
        
        Debug.Log($"Difficulty set to level {difficultyLevel}");
    }
    
    /// <summary>
    /// Gets the current difficulty level (method with return value)
    /// </summary>
    /// <returns>Current difficulty level</returns>
    public int GetDifficulty()
    {
        return difficultyLevel;
    }
    
    /// <summary>
    /// Calculates score multiplier based on time remaining (method with return value)
    /// </summary>
    /// <returns>Score multiplier</returns>
    public float CalculateScoreMultiplier()
    {
        if (maxGameTime <= 0) return 1f;
        
        float timeRemaining = maxGameTime - gameTimer;
        float multiplier = 1f + (timeRemaining / maxGameTime);
        
        return Mathf.Clamp(multiplier, 1f, 2f);
    }
    
    /// <summary>
    /// Checks if win condition is met
    /// </summary>
    private void CheckWinCondition()
    {
        if (currentScore >= targetScore)
        {
            WinGame();
        }
    }
    
    /// <summary>
    /// Checks if lose condition is met
    /// </summary>
    private void CheckLoseCondition()
    {
        if (gameTimer >= maxGameTime)
        {
            LoseGame("Time's up!");
        }
        
        if (player != null && player.GetHealth() <= 0)
        {
            LoseGame("You died!");
        }
    }
    
    /// <summary>
    /// Handles win condition
    /// </summary>
    private void WinGame()
    {
        isGameActive = false;
        
        if (uiManager != null)
        {
            uiManager.ShowWinScreen(true);
        }
        
        Debug.Log("You Win!");
    }
    
    /// <summary>
    /// Handles lose condition
    /// </summary>
    /// <param name="reason">Reason for losing</param>
    public void LoseGame(string reason)
    {
        isGameActive = false;
        
        if (uiManager != null)
        {
            uiManager.ShowLoseScreen(true, reason);
        }
        
        Debug.Log($"Game Over: {reason}");
    }
    
    /// <summary>
    /// Clears all spawned objects
    /// </summary>
    private void ClearAllObjects()
    {
        // Clear enemies
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        activeEnemies.Clear();
        
        // Clear items
        foreach (GameObject item in activeItems)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        activeItems.Clear();
    }
    
    /// <summary>
    /// Gets the current game state (method with return value)
    /// </summary>
    /// <returns>True if game is active, false otherwise</returns>
    public bool IsGameActive()
    {
        return isGameActive;
    }
}

