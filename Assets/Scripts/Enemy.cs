using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;              
    public float rotationSpeed;          
    public float detectionRange;         
    public Vector3 currentPosition;      
    
    [Header("Combat Settings")]
    public int maxHealth;                
    public int currentHealth;             
    public int damage;                   
    public float attackCooldown;          
    
    [Header("AI Settings")]
    public bool isChasing;               
    public float wanderRadius;           
    public float wanderTimer;            
    
    [Header("Enemy Stats")]
    public string enemyType;             
    public int scoreValue;               
    public bool isAlive;                 
    
    // Private fields
    private Transform playerTransform;   
    private GameManager gameManager;     
    private float attackTimer;           
    private Vector3 wanderTarget;        
    private float speedMultiplier;      
    private Renderer enemyRenderer;     
    private Rigidbody rb;              

    [Header("Ground Settings")]
    public float groundHeight = 0.5f;    
    
    
    void Start()
    {
        InitializeEnemy();
    }
    
    
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
    
    
    public void InitializeEnemy()
    {
        moveSpeed = 3f;
        rotationSpeed = 5f;
        detectionRange = 10f;
        maxHealth = 10;
        currentHealth = maxHealth;
        damage = 90;
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
        enemyRenderer = GetComponentInChildren<Renderer>();
        ApplyVisualStyle();
        SetupPhysics();
        ClampToGround();
        
       
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
        
        gameManager = FindObjectOfType<GameManager>();
    }
    
    
    private void UpdatePosition()
    {
        currentPosition = transform.position;
        ClampToGround();
    }
    
   
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
    
    
    private void HandleMovement()
    {
        Vector3 targetPosition;
        
        if (isChasing && playerTransform != null)
        {
            targetPosition = playerTransform.position;
        }
        else
        {
            wanderTimer += Time.deltaTime;
            
            if (wanderTimer >= 3f || Vector3.Distance(transform.position, wanderTarget) < 0.5f)
            {
                wanderTarget = GetRandomWanderTarget();
                wanderTimer = 0f;
            }
            
            targetPosition = wanderTarget;
        }
        
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0f;
        
        if (direction.magnitude > 0.1f)
        {
            Vector3 movement = direction * moveSpeed * speedMultiplier * Time.deltaTime;
            transform.position += movement;
            
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    private Vector3 GetRandomWanderTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        Vector3 target = transform.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
        target.y = groundHeight;
        return target;
    }
    
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
    
    public void TakeDamage(int damageAmount, float knockbackForce)
    {
        TakeDamage(damageAmount);
        
        if (playerTransform != null)
        {
            Vector3 knockbackDirection = (transform.position - playerTransform.position).normalized;
            transform.position += knockbackDirection * knockbackForce;
        }
    }
    
    private void Die()
    {
        isAlive = false;
        
        if (gameManager != null)
        {
            gameManager.AddScore(scoreValue);
            gameManager.RemoveEnemy(gameObject);
        }
        
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            
        }
        
        Debug.Log($"{enemyType} enemy defeated! +{scoreValue} points");
        
        
        Destroy(gameObject, 0.1f);
    }
    
    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
        moveSpeed *= multiplier;
    }
    
    public void SetEnemyType(string type)
    {
        enemyType = type;
        
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
        ApplyVisualStyle();
    }
    
    public int GetHealth()
    {
        return currentHealth;
    }
    
    public int GetDamage()
    {
        return damage;
    }
    
    public float GetDistanceToPlayer()
    {
        if (playerTransform == null) return -1f;
        return Vector3.Distance(transform.position, playerTransform.position);
    }
    
    public bool CanAttack()
    {
        return attackTimer >= attackCooldown;
    }
    
    private void UpdateAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }
    
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
    
    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && CanAttack())
        {
            AttackPlayer();
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null && CanAttack())
        {
            AttackPlayer();
        }
    }
    
    private void ApplyVisualStyle()
    {
        if (enemyRenderer == null)
        {
            enemyRenderer = GetComponentInChildren<Renderer>();
        }

        if (enemyRenderer == null) return;

        Color color = Color.red;
        switch (enemyType)
        {
            case "Fast":
                color = new Color(1f, 0.4f, 0.2f);
                break;
            case "Tank":
                color = new Color(0.6f, 0.2f, 0.8f);
                break;
            default:
                color = Color.red;
                break;
        }

        enemyRenderer.material.color = color;
        if (enemyRenderer.material.HasProperty("_EmissionColor"))
        {
            enemyRenderer.material.EnableKeyword("_EMISSION");
            enemyRenderer.material.SetColor("_EmissionColor", color * 0.4f);
        }
    }

    private void SetupPhysics()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezePositionY |
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY |
                         RigidbodyConstraints.FreezeRotationZ;
    }

    private void ClampToGround()
    {
        Vector3 pos = transform.position;
        pos.y = groundHeight;
        transform.position = pos;
    }
}