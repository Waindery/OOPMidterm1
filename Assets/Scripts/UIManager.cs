using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public Text scoreText;
    public Text healthText;
    public Text timerText;
    public Text gameOverText;
    public Text winText;
    
    [Header("UI Panels")]
    public GameObject gameUIPanel;
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    
    [Header("UI Buttons")]
    public Button startButton;
    public Button restartButton;
    public Button difficultyButton;
    
    [Header("UI Settings")]
    public string scorePrefix;
    public string healthPrefix;
    public string timerPrefix;
    public Color normalTextColor;
    public Color warningTextColor;
    
    [Header("UI Stats")]
    public int currentScoreDisplay;
    public int currentHealthDisplay;
    public float currentTimerDisplay;
    public bool isUIVisible;
    
    private GameManager gameManager;
    private Player player;
    private float uiUpdateInterval;
    private float uiUpdateTimer;
    
    void Start()
    {
        InitializeUI();
    }
    
    void Update()
    {
        UpdateUITimer();
    }
    
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
        
        gameManager = FindObjectOfType<GameManager>();
        player = FindObjectOfType<Player>();
        
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
        
        ShowStartScreen(true);
        ShowGameUI(false);
        ShowGameOverScreen(false);
        ShowWinScreen(false);
        
        if (startPanel == null && startButton != null)
        {
            startButton.gameObject.SetActive(true);
        }
        
        if (restartButton != null && startButton == null)
        {
            Debug.Log("No start button found. Restart button will work as start button.");
        }
    }
    
    private void UpdateUITimer()
    {
        uiUpdateTimer += Time.deltaTime;
    }
    
    public void UpdateScore(int score)
    {
        currentScoreDisplay = score;
        
        if (scoreText != null)
        {
            scoreText.text = scorePrefix + score.ToString();
        }
    }
    
    public void UpdateScore(int score, string prefix)
    {
        currentScoreDisplay = score;
        
        if (scoreText != null)
        {
            scoreText.text = prefix + score.ToString();
        }
    }
    
    public void UpdateHealth(int health, int maxHealth)
    {
        currentHealthDisplay = health;
        
        if (healthText != null)
        {
            healthText.text = healthPrefix + health.ToString() + " / " + maxHealth.ToString();
            
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
    
    public void UpdateTimer(float currentTime, float maxTime)
    {
        currentTimerDisplay = currentTime;
        
        if (timerText != null)
        {
            float remainingTime = maxTime - currentTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            
            timerText.text = timerPrefix + string.Format("{0:00}:{1:00}", minutes, seconds);
            
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
    
    public void ShowStartScreen(bool show)
    {
        if (startPanel != null)
        {
            startPanel.SetActive(show);
            FixPanelRaycast(startPanel, false);
        }
        
        if (show)
        {
            SetButtonVisibility(startButton, true);
            SetButtonVisibility(restartButton, false);
        }
    }
    
    public void ShowGameOverScreen(bool show)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(show);
        }
        
        if (show)
        {
            SetButtonVisibility(restartButton, true);
        }
    }
    
    public void ShowWinScreen(bool show)
    {
        if (winPanel != null)
        {
            winPanel.SetActive(show);
        }
        
        if (show)
        {
            SetButtonVisibility(restartButton, true);
        }
    }
    
    public void ShowWinScreen(bool show, string message)
    {
        ShowWinScreen(show);
        
        if (winText != null && show)
        {
            winText.text = message;
        }
    }
    
    public void ShowLoseScreen(bool show, string reason)
    {
        ShowGameOverScreen(show);
        
        if (gameOverText != null && show)
        {
            gameOverText.text = "Game Over: " + reason;
        }
    }
    
    public int GetCurrentScore()
    {
        return currentScoreDisplay;
    }
    
    public int GetCurrentHealth()
    {
        return currentHealthDisplay;
    }
    
    public float CalculateHealthPercentage(int health, int maxHealth)
    {
        if (maxHealth <= 0) return 0f;
        return Mathf.Clamp01((float)health / (float)maxHealth);
    }
    
    public void SetTextColor(Text textComponent, Color color)
    {
        if (textComponent != null)
        {
            textComponent.color = color;
        }
    }
    
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
            
            SetButtonVisibility(startButton, false);
            SetButtonVisibility(restartButton, false);
            
            ShowGameUI(true);
        }
        else
        {
            Debug.LogError("GameManager not found! Make sure GameManager exists in the scene.");
        }
    }
    
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
            if (!gameManager.IsGameActive())
            {
                Debug.Log("Game not active, starting game...");
                gameManager.StartGame();
                ShowStartScreen(false);
                SetButtonVisibility(restartButton, false);
            }
            else
            {
                Debug.Log("Game active, restarting...");
                gameManager.RestartGame();
                SetButtonVisibility(startButton, false);
                SetButtonVisibility(restartButton, false);
            }
            ShowGameOverScreen(false);
            ShowWinScreen(false);
            ShowGameUI(true);
        }
        else
        {
            Debug.LogError("GameManager not found! Make sure GameManager exists in the scene.");
        }
    }
    
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
    
    public void UpdateAllUI(int score, int health, int maxHealth, float timer, float maxTimer)
    {
        UpdateScore(score);
        UpdateHealth(health, maxHealth);
        UpdateTimer(timer, maxTimer);
    }
    
    private void FixPanelRaycast(GameObject panel, bool blockClicks)
    {
        if (panel == null) return;
        
        UnityEngine.UI.Image image = panel.GetComponent<UnityEngine.UI.Image>();
        if (image != null)
        {
            image.raycastTarget = blockClicks;
            if (!blockClicks)
            {
                Color panelColor = image.color;
                panelColor.a = 0f;
                image.color = panelColor;
            }
        }
    }
    
    private void SetButtonVisibility(Button button, bool show)
    {
        if (button != null)
        {
            button.gameObject.SetActive(show);
        }
    }
}

