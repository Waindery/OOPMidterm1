using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;               
    public float rotationSpeed;          
    public Vector3 currentVelocity;      
    
    [Header("Health Settings")]
    public int maxHealth;                
    public int currentHealth;             
    public float invincibilityDuration;  
    public bool isInvincible;            
    
    [Header("Combat Settings")]
    public int attackDamage;
    public float attackRange; 
    public float attackCooldown;        
    
    [Header("Player Stats")]
    public string playerName;
    public int itemsCollected;
    public int enemiesDefeated;
    
    // Private fields
    private float invincibilityTimer;
    private float attackTimer;
    private GameManager gameManager;
    private UIManager uiManager;
    private Rigidbody rb;
    
    void Start()
    {
        InitializePlayer();
    }
    
    void Update()
    {
        if (gameManager != null && gameManager.IsGameActive())
        {
            HandleMovement();
            HandleAttack();
            UpdateInvincibility();
        }
    }
    
    public void InitializePlayer()
    {
        moveSpeed = 25f;
        rotationSpeed = 25f;
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
        
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezePositionY |
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezeRotationZ;
        
        
        if (transform.position.y <= 0.1f)
        {
            Vector3 pos = transform.position;
            pos.y = 1f;
            transform.position = pos;
            Debug.Log("Player position adjusted to Y=1 to prevent falling through ground");
        }
        
        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
        
        if (uiManager != null)
        {
            uiManager.UpdateHealth(currentHealth, maxHealth);
        }
    }
    
    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        
        if (moveDirection.magnitude > 0.1f)
        {
            Vector3 movement = moveDirection * moveSpeed * Time.deltaTime;
            
            if (rb != null)
            {
                rb.MovePosition(transform.position + movement);
            }
            else
            {
                transform.position += movement;
            }
            
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            
            currentVelocity = moveDirection * moveSpeed;
        }
        else
        {
            currentVelocity = Vector3.zero;
        }
    }
    
    private void HandleAttack()
    {
        attackTimer += Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.Space) && attackTimer >= attackCooldown)
        {
            Attack();
            attackTimer = 0f;
        }
    }
    
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
    
    public void TakeDamage(int damage)
    {
        if (isInvincible) return;
        
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        
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
    
    public void CollectItem(Item item)
    {
        if (item == null) return;
        
        itemsCollected++;
        
        int itemValue = item.GetValue();
        string itemType = item.GetItemType();
        
        
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
    
    public int GetHealth()
    {
        return currentHealth;
    }
    
    public int GetMaxHealth()
    {
        return maxHealth;
    }
    
    public float GetHealthPercentage()
    {
        if (maxHealth <= 0) return 0f;
        return (float)currentHealth / (float)maxHealth;
    }
    
    public void SetPlayerName(string name)
    {
        playerName = name;
        Debug.Log($"Player name set to: {playerName}");
    }
    
    public void IncreaseSpeed(float speedBoost)
    {
        moveSpeed += speedBoost;
        Debug.Log($"Speed increased by {speedBoost}. New speed: {moveSpeed}");
    }
    
    private void Die()
    {
        Debug.Log($"{playerName} has died!");
        
        if (gameManager != null)
        {
            gameManager.LoseGame("Player died!");
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        Item item = other.GetComponent<Item>();
        if (item != null)
        {
            CollectItem(item);
            item.Collect();
        }
        
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null && !isInvincible)
        {
            TakeDamage(enemy.GetDamage());
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy != null && !isInvincible)
        {
            TakeDamage(enemy.GetDamage());
        }
    }
    
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

