using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enemy class handles enemy AI behavior, movement, health, and interactions.
/// Demonstrates OOP concepts: encapsulation, method parameters, return values, and class interactions.
/// </summary>
public class Enemy : MonoBehaviour
{
    // Fields (minimum 5 required)
    [Header("Movement Settings")]
    public float moveSpeed;              // Speed of enemy movement
    public float rotationSpeed;          // Speed of enemy rotation
    public float detectionRange;         // Range at which enemy detects player
    public Vector3 currentPosition;      // Current position of enemy
    
    [Header("Combat Settings")]
    public int maxHealth;                // Maximum health points
    public int currentHealth;             // Current health points
    public int damage;                   // Damage dealt to player
    public float attackCooldown;          // Cooldown between attacks
    
    [Header("AI Settings")]
    public bool isChasing;               // Whether enemy is chasing player
    public float wanderRadius;           // Radius for wandering behavior
    public float wanderTimer;            // Timer for wandering
    
    [Header("Enemy Stats")]
    public string enemyType;             // Type of enemy
    public int scoreValue;               // Score awarded when defeated
    public bool isAlive;                 // Whether enemy is alive
    
    // Private fields
    private Transform playerTransform;   // Reference to player's transform
    private GameManager gameManager;     // Reference to GameManager
    private float attackTimer;           // Timer for attack cooldown
    private Vector3 wanderTarget;        // Target position for wandering
    private float speedMultiplier;       // Speed multiplier from GameManager
    
    /// <summary>
    /// Unity Start method - initializes the enemy
    /// </summary>
    void Start()
    {
        InitializeEnemy();
    }
    
    /// <summary>
    /// Unity Update method - handles enemy AI and movement
    /// </summary>
    void Update()
    {
        if (isAlive && gameManager != null && gameManager.IsGameActive())
        {
            UpdatePosition();
            CheckForPlayer();
            HandleMovement();
            UpdateAttackTimer();
        }
    }
    
    /// <summary>
    /// Initializes the enemy with default values
    /// </summary>
    public void InitializeEnemy()
    {
        moveSpeed = 3f;
        rotationSpeed = 5f;
        detectionRange = 10f;
        maxHealth = 50;
        currentHealth = maxHealth;
        damage = 10;
        attackCooldown = 1f;
        isChasing = false;
        wanderRadius = 5f;
        wanderTimer = 0f;
        enemyType = "Basic";
        scoreValue = 10;
        isAlive = true;
        
        attackTimer = 0f;
        speedMultiplier = 1f;
        currentPosition = transform.position;
        wanderTarget = GetRandomWanderTarget();
        
        // Find references
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        
        gameManager = FindObjectOfType<GameManager>();
    }
    
    /// <summary>
    /// Updates the current position
    /// </summary>
    private void UpdatePosition()
    {
        currentPosition = transform.position;
    }
    
    /// <summary>
    /// Checks if player is within detection range
    /// </summary>
    private void CheckForPlayer()
    {
        if (playerTransform == null) return;
        
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        
        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }
        else if (distanceToPlayer > detectionRange * 1.5f)
        {
            isChasing = false;
        }
    }
    
    /// <summary>
    /// Handles enemy movement (chasing or wandering)
    /// </summary>
    private void HandleMovement()
    {
        Vector3 targetPosition;
        
        if (isChasing && playerTransform != null)
        {
            // Chase player
            targetPosition = playerTransform.position;
        }
        else
        {
            // Wander behavior
            wanderTimer += Time.deltaTime;
            
            if (wanderTimer >= 3f || Vector3.Distance(transform.position, wanderTarget) < 0.5f)
            {
                wanderTarget = GetRandomWanderTarget();
                wanderTimer = 0f;
            }
            
            targetPosition = wanderTarget;
        }
        
        // Move towards target
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f; // Keep enemy on ground
        
        if (direction.magnitude > 0.1f)
        {
            Vector3 movement = direction * moveSpeed * speedMultiplier * Time.deltaTime;
            transform.position += movement;
            
            // Rotate towards movement direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// Gets a random wander target position
    /// </summary>
    /// <returns>Random Vector3 position within wander radius</returns>
    private Vector3 GetRandomWanderTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        Vector3 target = transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
        return target;
    }
    
    /// <summary>
    /// Enemy takes damage (method with parameters)
    /// </summary>
    /// <param name="damageAmount">Amount of damage to take</param>
    public void TakeDamage(int damageAmount)
    {
        if (!isAlive) return;
        
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(0, currentHealth);
        
        Debug.Log($"{enemyType} enemy took {damageAmount} damage! Health: {currentHealth}/{maxHealth}");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    /// <summary>
    /// Overloaded method: Takes damage with knockback
    /// </summary>
    /// <param name="damageAmount">Amount of damage</param>
    /// <param name="knockbackForce">Knockback force</param>
    public void TakeDamage(int damageAmount, float knockbackForce)
    {
        TakeDamage(damageAmount);
        
        if (playerTransform != null)
        {
            Vector3 knockbackDirection = (transform.position - playerTransform.position).normalized;
            transform.position += knockbackDirection * knockbackForce;
        }
    }
    
    /// <summary>
    /// Handles enemy death
    /// </summary>
    private void Die()
    {
        isAlive = false;
        
        // Award score
        if (gameManager != null)
        {
            gameManager.AddScore(scoreValue);
            gameManager.RemoveEnemy(gameObject);
        }
        
        // Find player and increment defeated count
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            // Player's enemiesDefeated would be incremented through gameManager
        }
        
        Debug.Log($"{enemyType} enemy defeated! +{scoreValue} points");
        
        // Destroy enemy after a short delay
        Destroy(gameObject, 0.1f);
    }
    
    /// <summary>
    /// Sets the speed multiplier (method with parameters)
    /// </summary>
    /// <param name="multiplier">Speed multiplier value</param>
    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
        moveSpeed *= multiplier;
    }
    
    /// <summary>
    /// Sets enemy type (method with parameters)
    /// </summary>
    /// <param name="type">Type of enemy</param>
    public void SetEnemyType(string type)
    {
        enemyType = type;
        
        // Adjust stats based on type
        switch (type)
        {
            case "Fast":
                moveSpeed = 5f;
                maxHealth = 30;
                scoreValue = 15;
                break;
            case "Tank":
                moveSpeed = 2f;
                maxHealth = 100;
                scoreValue = 20;
                break;
            default:
                moveSpeed = 3f;
                maxHealth = 50;
                scoreValue = 10;
                break;
        }
        
        currentHealth = maxHealth;
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
    /// Gets damage value (method with return value)
    /// </summary>
    /// <returns>Damage value</returns>
    public int GetDamage()
    {
        return damage;
    }
    
    /// <summary>
    /// Gets distance to player (method with return value)
    /// </summary>
    /// <returns>Distance to player, or -1 if player not found</returns>
    public float GetDistanceToPlayer()
    {
        if (playerTransform == null) return -1f;
        return Vector3.Distance(transform.position, playerTransform.position);
    }
    
    /// <summary>
    /// Checks if enemy can attack (method with return value)
    /// </summary>
    /// <returns>True if attack is ready, false otherwise</returns>
    public bool CanAttack()
    {
        return attackTimer >= attackCooldown;
    }
    
    /// <summary>
    /// Updates attack timer
    /// </summary>
    private void UpdateAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }
    
    /// <summary>
    /// Attacks the player if in range
    /// </summary>
    public void AttackPlayer()
    {
        if (!CanAttack() || playerTransform == null) return;
        
        float distance = GetDistanceToPlayer();
        
        if (distance <= 2f)
        {
            Player player = playerTransform.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
                attackTimer = 0f;
            }
        }
    }
    
    /// <summary>
    /// Unity OnTriggerEnter - handles collisions with triggers
    /// </summary>
    /// <param name="other">Collider that entered the trigger</param>
    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && CanAttack())
        {
            AttackPlayer();
        }
    }
    
    /// <summary>
    /// Unity OnCollisionEnter - handles collisions with colliders
    /// </summary>
    /// <param name="collision">Collision information</param>
    void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null && CanAttack())
        {
            AttackPlayer();
        }
    }
}

