using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIManager class handles all UI updates and interactions.
/// Demonstrates OOP concepts: encapsulation, method parameters, return values, and class interactions.
/// </summary>
public class UIManager : MonoBehaviour
{
    // Fields (minimum 5 required)
    [Header("UI References")]
    public Text scoreText;               // Text component for score display
    public Text healthText;              // Text component for health display
    public Text timerText;               // Text component for timer display
    public Text gameOverText;            // Text component for game over message
    public Text winText;                 // Text component for win message
    
    [Header("UI Panels")]
    public GameObject gameUIPanel;       // Panel for in-game UI
    public GameObject startPanel;        // Panel for start screen
    public GameObject gameOverPanel;     // Panel for game over screen
    public GameObject winPanel;          // Panel for win screen
    
    [Header("UI Buttons")]
    public Button startButton;           // Button to start the game
    public Button restartButton;         // Button to restart the game
    public Button difficultyButton;      // Button to change difficulty
    
    [Header("UI Settings")]
    public string scorePrefix;           // Prefix for score text
    public string healthPrefix;          // Prefix for health text
    public string timerPrefix;           // Prefix for timer text
    public Color normalTextColor;        // Normal text color
    public Color warningTextColor;      // Warning text color (low health/time)
    
    [Header("UI Stats")]
    public int currentScoreDisplay;      // Currently displayed score
    public int currentHealthDisplay;     // Currently displayed health
    public float currentTimerDisplay;    // Currently displayed timer
    public bool isUIVisible;             // Whether UI is currently visible
    
    // Private fields
    private GameManager gameManager;     // Reference to GameManager
    private Player player;               // Reference to Player
    private float uiUpdateInterval;      // Interval for UI updates
    private float uiUpdateTimer;         // Timer for UI updates
    
    /// <summary>
    /// Unity Start method - initializes the UI
    /// </summary>
    void Start()
    {
        InitializeUI();
    }
    
    /// <summary>
    /// Unity Update method - handles UI updates
    /// </summary>
    void Update()
    {
        UpdateUITimer();
    }
    
    /// <summary>
    /// Initializes the UI with default values
    /// </summary>
    public void InitializeUI()
    {
        scorePrefix = "Score: ";
        healthPrefix = "Health: ";
        timerPrefix = "Time: ";
        normalTextColor = Color.white;
        warningTextColor = Color.red;
        isUIVisible = false;
        currentScoreDisplay = 0;
        currentHealthDisplay = 100;
        currentTimerDisplay = 0f;
        uiUpdateInterval = 0.1f;
        uiUpdateTimer = 0f;
        
        // Find references
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<Player>();
        
        // Setup button listeners
        if (startButton != null)
        {
            startButton.onClick.AddListener(OnStartButtonClicked);
        }
        
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartButtonClicked);
        }
        
        if (difficultyButton != null)
        {
            difficultyButton.onClick.AddListener(OnDifficultyButtonClicked);
        }
        
        // Show start panel initially (if it exists)
        ShowStartScreen(true);
        ShowGameUI(false);
        ShowGameOverScreen(false);
        ShowWinScreen(false);
        
        // If no start panel exists but we have a start button, make sure it's visible
        if (startPanel == null && startButton != null)
        {
            startButton.gameObject.SetActive(true);
        }
        
        // If restart button exists but no panels, make it work as start button too
        if (restartButton != null && startButton == null)
        {
            // Restart button can act as start button if no start button exists
            Debug.Log("No start button found. Restart button will work as start button.");
        }
    }
    
    /// <summary>
    /// Updates UI timer for periodic updates
    /// </summary>
    private void UpdateUITimer()
    {
        uiUpdateTimer += Time.deltaTime;
    }
    
    /// <summary>
    /// Updates the score display (method with parameters)
    /// </summary>
    /// <param name="score">Current score value</param>
    public void UpdateScore(int score)
    {
        currentScoreDisplay = score;
        
        if (scoreText != null)
        {
            scoreText.text = scorePrefix + score.ToString();
        }
    }
    
    /// <summary>
    /// Overloaded method: Updates score with custom prefix
    /// </summary>
    /// <param name="score">Current score value</param>
    /// <param name="prefix">Custom prefix text</param>
    public void UpdateScore(int score, string prefix)
    {
        currentScoreDisplay = score;
        
        if (scoreText != null)
        {
            scoreText.text = prefix + score.ToString();
        }
    }
    
    /// <summary>
    /// Updates the health display (method with parameters)
    /// </summary>
    /// <param name="health">Current health value</param>
    /// <param name="maxHealth">Maximum health value</param>
    public void UpdateHealth(int health, int maxHealth)
    {
        currentHealthDisplay = health;
        
        if (healthText != null)
        {
            healthText.text = healthPrefix + health.ToString() + " / " + maxHealth.ToString();
            
            // Change color if health is low
            float healthPercentage = (float)health / (float)maxHealth;
            if (healthPercentage < 0.3f)
            {
                healthText.color = warningTextColor;
            }
            else
            {
                healthText.color = normalTextColor;
            }
        }
    }
    
    /// <summary>
    /// Updates the timer display (method with parameters)
    /// </summary>
    /// <param name="currentTime">Current time value</param>
    /// <param name="maxTime">Maximum time value</param>
    public void UpdateTimer(float currentTime, float maxTime)
    {
        currentTimerDisplay = currentTime;
        
        if (timerText != null)
        {
            float remainingTime = maxTime - currentTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            
            timerText.text = timerPrefix + string.Format("{0:00}:{1:00}", minutes, seconds);
            
            // Change color if time is running out
            float timePercentage = remainingTime / maxTime;
            if (timePercentage < 0.2f)
            {
                timerText.color = warningTextColor;
            }
            else
            {
                timerText.color = normalTextColor;
            }
        }
    }
    
    /// <summary>
    /// Shows or hides the game UI panel (method with parameters)
    /// </summary>
    /// <param name="show">Whether to show the UI</param>
    public void ShowGameUI(bool show)
    {
        isUIVisible = show;
        
        if (gameUIPanel != null)
        {
            gameUIPanel.SetActive(show);
            // Fix panel to not block clicks
            FixPanelRaycast(gameUIPanel, false);
        }
    }
    
    /// <summary>
    /// Shows or hides the start screen (method with parameters)
    /// </summary>
    /// <param name="show">Whether to show the screen</param>
    public void ShowStartScreen(bool show)
    {
        if (startPanel != null)
        {
            startPanel.SetActive(show);
            // Fix panel to not block clicks if it's just a background
            FixPanelRaycast(startPanel, false);
        }
    }
    
    /// <summary>
    /// Shows or hides the game over screen (method with parameters)
    /// </summary>
    /// <param name="show">Whether to show the screen</param>
    public void ShowGameOverScreen(bool show)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(show);
        }
    }
    
    /// <summary>
    /// Shows or hides the win screen (method with parameters)
    /// </summary>
    /// <param name="show">Whether to show the screen</param>
    public void ShowWinScreen(bool show)
    {
        if (winPanel != null)
        {
            winPanel.SetActive(show);
        }
    }
    
    /// <summary>
    /// Overloaded method: Shows win screen with message
    /// </summary>
    /// <param name="show">Whether to show the screen</param>
    /// <param name="message">Win message</param>
    public void ShowWinScreen(bool show, string message)
    {
        ShowWinScreen(show);
        
        if (winText != null && show)
        {
            winText.text = message;
        }
    }
    
    /// <summary>
    /// Shows lose screen with message (method with parameters)
    /// </summary>
    /// <param name="show">Whether to show the screen</param>
    /// <param name="reason">Reason for losing</param>
    public void ShowLoseScreen(bool show, string reason)
    {
        ShowGameOverScreen(show);
        
        if (gameOverText != null && show)
        {
            gameOverText.text = "Game Over: " + reason;
        }
    }
    
    /// <summary>
    /// Gets current score display (method with return value)
    /// </summary>
    /// <returns>Currently displayed score</returns>
    public int GetCurrentScore()
    {
        return currentScoreDisplay;
    }
    
    /// <summary>
    /// Gets current health display (method with return value)
    /// </summary>
    /// <returns>Currently displayed health</returns>
    public int GetCurrentHealth()
    {
        return currentHealthDisplay;
    }
    
    /// <summary>
    /// Calculates health percentage (method with return value)
    /// </summary>
    /// <param name="health">Current health</param>
    /// <param name="maxHealth">Maximum health</param>
    /// <returns>Health percentage (0-1)</returns>
    public float CalculateHealthPercentage(int health, int maxHealth)
    {
        if (maxHealth <= 0) return 0f;
        return Mathf.Clamp01((float)health / (float)maxHealth);
    }
    
    /// <summary>
    /// Sets text color (method with parameters)
    /// </summary>
    /// <param name="textComponent">Text component to modify</param>
    /// <param name="color">Color to set</param>
    public void SetTextColor(Text textComponent, Color color)
    {
        if (textComponent != null)
        {
            textComponent.color = color;
        }
    }
    
    /// <summary>
    /// Handles start button click (public for Unity Inspector)
    /// </summary>
    public void OnStartButtonClicked()
    {
        Debug.Log("Start Button Clicked!");
        
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
            Debug.Log("Searching for GameManager...");
        }
        
        if (gameManager != null)
        {
            Debug.Log("GameManager found! Starting game...");
            gameManager.StartGame();
            ShowStartScreen(false);
            ShowGameOverScreen(false);
            ShowWinScreen(false);
            
            // Hide start button
            if (startButton != null)
            {
                startButton.gameObject.SetActive(false);
            }
            
            // Show game UI elements
            ShowGameUI(true);
        }
        else
        {
            Debug.LogError("GameManager not found! Make sure GameManager exists in the scene.");
        }
    }
    
    /// <summary>
    /// Handles restart button click (public for Unity Inspector)
    /// </summary>
    public void OnRestartButtonClicked()
    {
        Debug.Log("Restart Button Clicked!");
        
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
            Debug.Log("Searching for GameManager...");
        }
        
        if (gameManager != null)
        {
            Debug.Log("GameManager found!");
            // If game hasn't started yet, start it. Otherwise restart.
            if (!gameManager.IsGameActive())
            {
                Debug.Log("Game not active, starting game...");
                gameManager.StartGame();
                ShowStartScreen(false);
                
                // Hide restart button (it will show again when game ends)
                if (restartButton != null)
                {
                    restartButton.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.Log("Game active, restarting...");
                gameManager.RestartGame();
            }
            ShowGameOverScreen(false);
            ShowWinScreen(false);
            
            // Show game UI
            ShowGameUI(true);
        }
        else
        {
            Debug.LogError("GameManager not found! Make sure GameManager exists in the scene.");
        }
    }
    
    /// <summary>
    /// Handles difficulty button click (public for Unity Inspector)
    /// </summary>
    public void OnDifficultyButtonClicked()
    {
        if (gameManager != null)
        {
            int currentDifficulty = gameManager.GetDifficulty();
            int newDifficulty = (currentDifficulty % 3) + 1;
            gameManager.SetDifficulty(newDifficulty);
            
            Debug.Log($"Difficulty changed to level {newDifficulty}");
        }
    }
    
    /// <summary>
    /// Updates all UI elements at once (method with parameters)
    /// </summary>
    /// <param name="score">Score value</param>
    /// <param name="health">Health value</param>
    /// <param name="maxHealth">Max health value</param>
    /// <param name="timer">Timer value</param>
    /// <param name="maxTimer">Max timer value</param>
    public void UpdateAllUI(int score, int health, int maxHealth, float timer, float maxTimer)
    {
        UpdateScore(score);
        UpdateHealth(health, maxHealth);
        UpdateTimer(timer, maxTimer);
    }
    
    /// <summary>
    /// Fixes panel raycast target to prevent blocking clicks (method with parameters)
    /// </summary>
    /// <param name="panel">Panel GameObject to fix</param>
    /// <param name="blockClicks">Whether panel should block clicks</param>
    private void FixPanelRaycast(GameObject panel, bool blockClicks)
    {
        if (panel == null) return;
        
        UnityEngine.UI.Image image = panel.GetComponent<UnityEngine.UI.Image>();
        if (image != null)
        {
            image.raycastTarget = blockClicks;
            // If not blocking, make it transparent
            if (!blockClicks)
            {
                Color panelColor = image.color;
                panelColor.a = 0f; // Fully transparent
                image.color = panelColor;
            }
        }
    }
}

