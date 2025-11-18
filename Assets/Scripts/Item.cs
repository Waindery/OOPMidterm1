using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Item class handles collectible items that can be picked up by the player.
/// Demonstrates OOP concepts: encapsulation, method parameters, return values, and class interactions.
/// </summary>
public class Item : MonoBehaviour
{
    // Fields (minimum 5 required)
    [Header("Item Properties")]
    public string itemType;              // Type of item (Health, Score, PowerUp)
    public int value;                    // Value of the item
    public string itemName;               // Name of the item
    public Color itemColor;               // Color of the item
    
    [Header("Item Behavior")]
    public float rotationSpeed;           // Speed of item rotation
    public float floatSpeed;              // Speed of floating animation
    public float floatAmplitude;         // Amplitude of floating animation
    public bool isCollected;              // Whether item has been collected
    
    [Header("Item Effects")]
    public int healthRestore;             // Health restored (if health item)
    public int scorePoints;               // Score points (if score item)
    public float effectDuration;          // Duration of effect (if power-up)
    
    [Header("Item Stats")]
    public float spawnTime;               // Time when item was spawned
    public float lifetime;                // Maximum lifetime of item
    public bool despawnsOverTime;         // Whether item despawns after lifetime
    
    // Private fields
    private Vector3 startPosition;        // Starting position for floating animation
    private float floatTimer;            // Timer for floating animation
    private GameManager gameManager;      // Reference to GameManager
    private Renderer itemRenderer;        // Renderer component
    
    /// <summary>
    /// Unity Start method - initializes the item
    /// </summary>
    void Start()
    {
        InitializeItem();
    }
    
    /// <summary>
    /// Unity Update method - handles item animation and lifetime
    /// </summary>
    void Update()
    {
        if (!isCollected)
        {
            RotateItem();
            FloatAnimation();
            CheckLifetime();
        }
    }
    
    /// <summary>
    /// Initializes the item with default or random values
    /// </summary>
    public void InitializeItem()
    {
        spawnTime = Time.time;
        floatTimer = 0f;
        isCollected = false;
        startPosition = transform.position;
        
        // Get components
        itemRenderer = GetComponent<Renderer>();
        
        // Find references
        gameManager = FindObjectOfType<GameManager>();
        
        // Set default values if not set in inspector
        if (string.IsNullOrEmpty(itemType))
        {
            SetRandomItemType();
        }
        
        if (value <= 0)
        {
            SetValueBasedOnType();
        }
        
        // Apply visual properties
        ApplyVisualProperties();
    }
    
    /// <summary>
    /// Sets a random item type
    /// </summary>
    private void SetRandomItemType()
    {
        float random = Random.Range(0f, 1f);
        
        if (random < 0.5f)
        {
            itemType = "Score";
        }
        else
        {
            itemType = "Health";
        }
    }
    
    /// <summary>
    /// Sets value based on item type
    /// </summary>
    private void SetValueBasedOnType()
    {
        switch (itemType)
        {
            case "Health":
                value = Random.Range(10, 30);
                healthRestore = value;
                itemName = "Health Potion";
                break;
            case "Score":
                value = Random.Range(5, 15);
                scorePoints = value;
                itemName = "Score Gem";
                break;
            case "PowerUp":
                value = 1;
                effectDuration = 5f;
                itemName = "Power Up";
                break;
            default:
                value = 10;
                itemName = "Item";
                break;
        }
    }
    
    /// <summary>
    /// Applies visual properties based on item type
    /// </summary>
    private void ApplyVisualProperties()
    {
        if (itemRenderer == null) return;
        
        switch (itemType)
        {
            case "Health":
                itemColor = Color.green;
                break;
            case "Score":
                itemColor = Color.yellow;
                break;
            case "PowerUp":
                itemColor = Color.blue;
                break;
            default:
                itemColor = Color.white;
                break;
        }
        
        itemRenderer.material.color = itemColor;
    }
    
    /// <summary>
    /// Rotates the item
    /// </summary>
    private void RotateItem()
    {
        if (rotationSpeed > 0)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
    
    /// <summary>
    /// Handles floating animation
    /// </summary>
    private void FloatAnimation()
    {
        if (floatSpeed > 0)
        {
            floatTimer += Time.deltaTime;
            float yOffset = Mathf.Sin(floatTimer * floatSpeed) * floatAmplitude;
            transform.position = startPosition + new Vector3(0f, yOffset, 0f);
        }
    }
    
    /// <summary>
    /// Checks if item should despawn due to lifetime
    /// </summary>
    private void CheckLifetime()
    {
        if (despawnsOverTime && lifetime > 0)
        {
            if (Time.time - spawnTime >= lifetime)
            {
                Despawn();
            }
        }
    }
    
    /// <summary>
    /// Sets item type (method with parameters)
    /// </summary>
    /// <param name="type">Type of item</param>
    public void SetItemType(string type)
    {
        itemType = type;
        SetValueBasedOnType();
        ApplyVisualProperties();
    }
    
    /// <summary>
    /// Sets item value (method with parameters)
    /// </summary>
    /// <param name="newValue">New value for the item</param>
    public void SetValue(int newValue)
    {
        value = newValue;
        
        if (itemType == "Health")
        {
            healthRestore = value;
        }
        else if (itemType == "Score")
        {
            scorePoints = value;
        }
    }
    
    /// <summary>
    /// Overloaded method: Sets value and type
    /// </summary>
    /// <param name="type">Type of item</param>
    /// <param name="newValue">Value of item</param>
    public void SetValue(string type, int newValue)
    {
        SetItemType(type);
        SetValue(newValue);
    }
    
    /// <summary>
    /// Gets the item value (method with return value)
    /// </summary>
    /// <returns>Item value</returns>
    public int GetValue()
    {
        return value;
    }
    
    /// <summary>
    /// Gets the item type (method with return value)
    /// </summary>
    /// <returns>Item type string</returns>
    public string GetItemType()
    {
        return itemType;
    }
    
    /// <summary>
    /// Gets the item name (method with return value)
    /// </summary>
    /// <returns>Item name string</returns>
    public string GetItemName()
    {
        return itemName;
    }
    
    /// <summary>
    /// Calculates remaining lifetime (method with return value)
    /// </summary>
    /// <returns>Remaining lifetime in seconds</returns>
    public float GetRemainingLifetime()
    {
        if (!despawnsOverTime || lifetime <= 0) return -1f;
        return Mathf.Max(0f, lifetime - (Time.time - spawnTime));
    }
    
    /// <summary>
    /// Collects the item
    /// </summary>
    public void Collect()
    {
        if (isCollected) return;
        
        isCollected = true;
        
        // Remove from GameManager
        if (gameManager != null)
        {
            gameManager.RemoveItem(gameObject);
        }
        
        // Play collection effect (could add particle effects here)
        Debug.Log($"Item collected: {itemName} (Type: {itemType}, Value: {value})");
        
        // Destroy the item
        Destroy(gameObject, 0.1f);
    }
    
    /// <summary>
    /// Despawns the item
    /// </summary>
    private void Despawn()
    {
        if (gameManager != null)
        {
            gameManager.RemoveItem(gameObject);
        }
        
        Destroy(gameObject);
    }
    
    /// <summary>
    /// Checks if item is collectible (method with return value)
    /// </summary>
    /// <returns>True if item can be collected, false otherwise</returns>
    public bool IsCollectible()
    {
        return !isCollected && gameObject.activeSelf;
    }
    
    /// <summary>
    /// Sets rotation speed (method with parameters)
    /// </summary>
    /// <param name="speed">Rotation speed value</param>
    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }
    
    /// <summary>
    /// Sets floating animation parameters (method with parameters)
    /// </summary>
    /// <param name="speed">Float speed</param>
    /// <param name="amplitude">Float amplitude</param>
    public void SetFloatAnimation(float speed, float amplitude)
    {
        floatSpeed = speed;
        floatAmplitude = amplitude;
    }
    
    /// <summary>
    /// Unity OnTriggerEnter - handles player collection
    /// </summary>
    /// <param name="other">Collider that entered the trigger</param>
    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && IsCollectible())
        {
            player.CollectItem(this);
        }
    }
}

