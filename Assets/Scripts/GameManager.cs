using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float gameTimer;              
    public float maxGameTime;            
    public int currentScore;              
    public int targetScore;               
    public bool isGameActive;             
    
    [Header("Spawn Settings")]
    public GameObject enemyPrefab;        
    public GameObject itemPrefab;         
    public float spawnInterval;          
    public int maxEnemies;                
    public int maxItems;                  
    
    [Header("Difficulty Settings")]
    public int difficultyLevel;           
    public float enemySpeedMultiplier;    
    public float spawnRateMultiplier;     
    
    private float spawnTimer;            
    private List<GameObject> activeEnemies; 
    private List<GameObject> activeItems;   
    private UIManager uiManager;           
    private Player player;                 
    
    void Start()
    {
        InitializeGame();
    }
    
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
    
    public void InitializeGame()
    {
        gameTimer = 0f;
        maxGameTime = 60f;
        currentScore = 0;
        targetScore = 150;
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
        
        uiManager = FindObjectOfType<UIManager>();
        player = FindObjectOfType<Player>();
        
        if (uiManager != null)
        {
            uiManager.UpdateScore(currentScore);
            uiManager.UpdateTimer(gameTimer, maxGameTime);
        }
        
        Debug.Log("Game initialized. Press Start to begin!");
    }
    
    public void StartGame()
    {
        Debug.Log("GameManager.StartGame() called!");
        isGameActive = true;
        gameTimer = 0f;
        currentScore = 0;
        
        ClearAllObjects();
        
        if (uiManager != null)
        {
            Debug.Log("Updating UI...");
            uiManager.ShowGameUI(true);
            uiManager.UpdateScore(currentScore);
            uiManager.UpdateTimer(gameTimer, maxGameTime);
        }
        else
        {
            Debug.LogWarning("UIManager is null!");
        }
        
        if (player != null)
        {
            player.ResetPlayer();
        }
        
        Debug.Log("Game Started! isGameActive = " + isGameActive);
    }
    
    public void StopGame()
    {
        isGameActive = false;
        ClearAllObjects();
        
        if (uiManager != null)
        {
            uiManager.ShowGameUI(false);
        }
    }
    
    public void RestartGame()
    {
        StopGame();
        InitializeGame();
        StartGame();
    }
    
    private void UpdateGameTimer()
    {
        gameTimer += Time.deltaTime;
        
        if (uiManager != null)
        {
            uiManager.UpdateTimer(gameTimer, maxGameTime);
        }
    }
    
    private void HandleSpawning()
    {
        spawnTimer += Time.deltaTime;
        float adjustedSpawnInterval = spawnInterval / spawnRateMultiplier;
        
        if (spawnTimer >= adjustedSpawnInterval)
        {
            spawnTimer = 0f;
            
            if (activeEnemies.Count < maxEnemies)
            {
                SpawnEnemy();
            }
            
            if (activeItems.Count < maxItems)
            {
                SpawnItem();
            }
        }
    }
    
    private void SpawnEnemy()
    {
        if (enemyPrefab == null) return;
        
        Vector3 spawnPosition = GetRandomSpawnPosition(1.0f);
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.SetSpeedMultiplier(enemySpeedMultiplier);
        }
        
        activeEnemies.Add(enemy);
    }
    
    private void SpawnItem()
    {
        if (itemPrefab == null) return;
        
        Vector3 spawnPosition = GetRandomSpawnPosition(0.75f);
        GameObject item = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        activeItems.Add(item);
    }
    
    private Vector3 GetRandomSpawnPosition(float height = 0.5f)
    {
        float x = Random.Range(-8f, 8f);
        float z = Random.Range(-8f, 8f);
        return new Vector3(x, height, z);
    }
    
    public void AddScore(int points)
    {
        currentScore += points;
        
        if (uiManager != null)
        {
            uiManager.UpdateScore(currentScore);
        }
        
        Debug.Log($"Score added: {points}. Total: {currentScore}");
    }
    
    public void AddScore(int points, float multiplier)
    {
        int finalPoints = Mathf.RoundToInt(points * multiplier);
        AddScore(finalPoints);
    }
    
    public void RemoveEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }
    
    public void RemoveItem(GameObject item)
    {
        if (activeItems.Contains(item))
        {
            activeItems.Remove(item);
        }
    }
    
    public void SetDifficulty(int level)
    {
        difficultyLevel = Mathf.Clamp(level, 1, 3);
        
        switch (difficultyLevel)
        {
            case 1:
                enemySpeedMultiplier = 1f;
                spawnRateMultiplier = 1f;
                targetScore = 250;
                break;
            case 2:
                enemySpeedMultiplier = 1.5f;
                spawnRateMultiplier = 1.3f;
                targetScore = 275;
                break;
            case 3:
                enemySpeedMultiplier = 2f;
                spawnRateMultiplier = 1.6f;
                targetScore = 300;
                break;
        }
        
        Debug.Log($"Difficulty set to level {difficultyLevel}");
    }
    
    public int GetDifficulty()
    {
        return difficultyLevel;
    }
    
    public float CalculateScoreMultiplier()
    {
        if (maxGameTime <= 0) return 1f;
        
        float timeRemaining = maxGameTime - gameTimer;
        float multiplier = 1f + (timeRemaining / maxGameTime);
        
        return Mathf.Clamp(multiplier, 1f, 2f);
    }
    
    private void CheckWinCondition()
    {
        if (currentScore >= targetScore)
        {
            WinGame();
        }
    }
    
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
    
    private void WinGame()
    {
        isGameActive = false;
        
        if (uiManager != null)
        {
            uiManager.ShowWinScreen(true);
        }
        
        Debug.Log("You Win!");
    }
    
    public void LoseGame(string reason)
    {
        isGameActive = false;
        
        if (uiManager != null)
        {
            uiManager.ShowLoseScreen(true, reason);
        }
        
        Debug.Log($"Game Over: {reason}");
    }
    
    private void ClearAllObjects()
    {
        foreach (GameObject enemy in activeEnemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        activeEnemies.Clear();
        
        foreach (GameObject item in activeItems)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        activeItems.Clear();
    }
    
    public bool IsGameActive()
    {
        return isGameActive;
    }
}