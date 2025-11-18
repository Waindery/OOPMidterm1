using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Player class handles player movement, actions, health, and interactions with other objects.
/// Demonstrates OOP concepts: encapsulation, method parameters, return values, and class interactions.
/// </summary>
public class Player : MonoBehaviour
{
    // Fields (minimum 5 required)
    [Header("Movement Settings")]
    public float moveSpeed;               // Speed of player movement
    public float rotationSpeed;          // Speed of player rotation
    public Vector3 currentVelocity;      // Current velocity vector
    
    [Header("Health Settings")]
    public int maxHealth;                // Maximum health points
    public int currentHealth;             // Current health points
    public float invincibilityDuration;  // Duration of invincibility after taking damage
    public bool isInvincible;            // Whether player is currently invincible
    
    [Header("Combat Settings")]
    public int attackDamage;             // Damage dealt by player
    public float attackRange;            // Range of player's attack
    public float attackCooldown;         // Cooldown between attacks
    
    [Header("Player Stats")]
    public string playerName;            // Player's name
    public int itemsCollected;           // Number of items collected
    public int enemiesDefeated;          // Number of enemies defeated
    
    // Private fields
    private float invincibilityTimer;    // Timer for invincibility
    private float attackTimer;           // Timer for attack cooldown
    private GameManager gameManager;     // Reference to GameManager
    private UIManager uiManager;         // Reference to UIManager
    private Rigidbody rb;                // Rigidbody component
    
    /// <summary>
    /// Unity Start method - initializes the player
    /// </summary>
    void Start()
    {
        InitializePlayer();
    }
    
    /// <summary>
    /// Unity Update method - handles player input and updates
    /// </summary>
    void Update()
    {
        if (gameManager != null && gameManager.IsGameActive())
        {
            HandleMovement();
            HandleAttack();
            UpdateInvincibility();
        }
    }
    
    /// <summary>
    /// Initializes the player with default values
    /// </summary>
    public void InitializePlayer()
    {
        moveSpeed = 5f;
        rotationSpeed = 10f;
        maxHealth = 100;
        currentHealth = maxHealth;
        invincibilityDuration = 1f;
        isInvincible = false;
        attackDamage = 10;
        attackRange = 2f;
        attackCooldown = 0.5f;
        playerName = "Player";
        itemsCollected = 0;
        enemiesDefeated = 0;
        
        invincibilityTimer = 0f;
        attackTimer = 0f;
        currentVelocity = Vector3.zero;
        
        // Get components
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.freezeRotation = true;
        }
        
        // Find references
        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
        
        if (uiManager != null)
        {
            uiManager.UpdateHealth(currentHealth, maxHealth);
        }
    }
    
    /// <summary>
    /// Handles player movement based on input
    /// </summary>
    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        
        if (moveDirection.magnitude > 0.1f)
        {
            // Move player
            Vector3 movement = moveDirection * moveSpeed * Time.deltaTime;
            transform.position += movement;
            
            // Rotate player to face movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            currentVelocity = moveDirection * moveSpeed;
        }
        else
        {
            currentVelocity = Vector3.zero;
        }
    }
    
    /// <summary>
    /// Handles player attack input
    /// </summary>
    private void HandleAttack()
    {
        attackTimer += Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Space) && attackTimer >= attackCooldown)
        {
            Attack();
            attackTimer = 0f;
        }
    }
    
    /// <summary>
    /// Player attacks nearby enemies (method with no parameters)
    /// </summary>
    public void Attack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        
        foreach (Collider col in hitColliders)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log($"{playerName} attacked enemy for {attackDamage} damage!");
            }
        }
    }
    
    /// <summary>
    /// Overloaded method: Player attacks with custom damage
    /// </summary>
    /// <param name="damage">Damage amount</param>
    public void Attack(int damage)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        
        foreach (Collider col in hitColliders)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"{playerName} attacked enemy for {damage} damage!");
            }
        }
    }
    
    /// <summary>
    /// Player takes damage (method with parameters)
    /// </summary>
    /// <param name="damage">Amount of damage to take</param>
    public void TakeDamage(int damage)
    {
        if (isInvincible) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        
        // Activate invincibility
        isInvincible = true;
        invincibilityTimer = 0f;
        
        if (uiManager != null)
        {
            uiManager.UpdateHealth(currentHealth, maxHealth);
        }
        
        Debug.Log($"{playerName} took {damage} damage! Health: {currentHealth}/{maxHealth}");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    /// <summary>
    /// Player heals (method with parameters)
    /// </summary>
    /// <param name="healAmount">Amount of health to restore</param>
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);
        
        if (uiManager != null)
        {
            uiManager.UpdateHealth(currentHealth, maxHealth);
        }
        
        Debug.Log($"{playerName} healed for {healAmount}! Health: {currentHealth}/{maxHealth}");
    }
    
    /// <summary>
    /// Updates invincibility timer
    /// </summary>
    private void UpdateInvincibility()
    {
        if (isInvincible)
        {
            invincibilityTimer += Time.deltaTime;
            
            if (invincibilityTimer >= invincibilityDuration)
            {
                isInvincible = false;
                invincibilityTimer = 0f;
            }
        }
    }
    
    /// <summary>
    /// Collects an item (method with parameters)
    /// </summary>
    /// <param name="item">Item that was collected</param>
    public void CollectItem(Item item)
    {
        if (item == null) return;
        
        itemsCollected++;
        
        // Get item value
        int itemValue = item.GetValue();
        string itemType = item.GetItemType();
        
        // Apply item effects
        if (itemType == "Health")
        {
            Heal(itemValue);
        }
        else if (itemType == "Score")
        {
            if (gameManager != null)
            {
                float multiplier = gameManager.CalculateScoreMultiplier();
                gameManager.AddScore(itemValue, multiplier);
            }
        }
        
        Debug.Log($"{playerName} collected {itemType} item worth {itemValue}!");
    }
    
    /// <summary>
    /// Gets current health (method with return value)
    /// </summary>
    /// <returns>Current health value</returns>
    public int GetHealth()
    {
        return currentHealth;
    }
    
    /// <summary>
    /// Gets maximum health (method with return value)
    /// </summary>
    /// <returns>Maximum health value</returns>
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    
    /// <summary>
    /// Calculates health percentage (method with return value)
    /// </summary>
    /// <returns>Health percentage (0-1)</returns>
    public float GetHealthPercentage()
    {
        if (maxHealth <= 0) return 0f;
        return (float)currentHealth / (float)maxHealth;
    }
    
    /// <summary>
    /// Sets player name (method with parameters)
    /// </summary>
    /// <param name="name">New player name</param>
    public void SetPlayerName(string name)
    {
        playerName = name;
        Debug.Log($"Player name set to: {playerName}");
    }
    
    /// <summary>
    /// Increases movement speed (method with parameters)
    /// </summary>
    /// <param name="speedBoost">Speed increase amount</param>
    public void IncreaseSpeed(float speedBoost)
    {
        moveSpeed += speedBoost;
        Debug.Log($"Speed increased by {speedBoost}. New speed: {moveSpeed}");
    }
    
    /// <summary>
    /// Handles player death
    /// </summary>
    private void Die()
    {
        Debug.Log($"{playerName} has died!");
        
        if (gameManager != null)
        {
            gameManager.LoseGame("Player died!");
        }
    }
    
    /// <summary>
    /// Unity OnTriggerEnter - handles collisions with triggers
    /// </summary>
    /// <param name="other">Collider that entered the trigger</param>
    void OnTriggerEnter(Collider other)
    {
        // Check for item collection
        Item item = other.GetComponent<Item>();
        if (item != null)
        {
            CollectItem(item);
            item.Collect();
        }
        
        // Check for enemy collision
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null && !isInvincible)
        {
            TakeDamage(enemy.GetDamage());
        }
    }
    
    /// <summary>
    /// Unity OnCollisionEnter - handles collisions with colliders
    /// </summary>
    /// <param name="collision">Collision information</param>
    void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null && !isInvincible)
        {
            TakeDamage(enemy.GetDamage());
        }
    }
    
    /// <summary>
    /// Resets player to initial state
    /// </summary>
    public void ResetPlayer()
    {
        currentHealth = maxHealth;
        itemsCollected = 0;
        enemiesDefeated = 0;
        isInvincible = false;
        invincibilityTimer = 0f;
        attackTimer = 0f;
        
        if (uiManager != null)
        {
            uiManager.UpdateHealth(currentHealth, maxHealth);
        }
    }
}

