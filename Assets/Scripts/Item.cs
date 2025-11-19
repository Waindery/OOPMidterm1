using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [Header("Item Properties")]
    public string itemType;
    public int value;                  
    public string itemName;              
    public Color itemColor;             
    
    [Header("Item Behavior")]
    public float rotationSpeed;           
    public float floatSpeed;             
    public float floatAmplitude;         
    public bool isCollected;              
    
    [Header("Item Effects")]
    public int healthRestore;             
    public int scorePoints;               
    public float effectDuration;          
    
    [Header("Item Stats")]
    public float spawnTime;               
    public float lifetime;                
    public bool despawnsOverTime;         
    
    private Vector3 startPosition;        
    private float floatTimer;            
    private GameManager gameManager;      
    private Renderer itemRenderer;        
    
    void Start()
    {
        InitializeItem();
    }
    
    void Update()
    {
        if (!isCollected)
        {
            RotateItem();
            FloatAnimation();
            CheckLifetime();
        }
    }
    
    public void InitializeItem()
    {
        spawnTime = Time.time;
        floatTimer = 0f;
        isCollected = false;
        startPosition = transform.position;
        
        itemRenderer = GetComponent<Renderer>();
        if (itemRenderer == null)
        {
            itemRenderer = GetComponentInChildren<Renderer>();
        }
        
        gameManager = FindObjectOfType<GameManager>();
        
        if (string.IsNullOrEmpty(itemType))
        {
            SetRandomItemType();
        }
        
        if (value <= 0)
        {
            SetValueBasedOnType();
        }
        
        ApplyVisualProperties();
    }
    
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
    
    private void ApplyVisualProperties()
    {
        if (itemRenderer == null) return;
        
        switch (itemType)
        {
            case "Health":
                itemColor = Color.green;
                break;
            case "Score":
                itemColor = new Color(1f, 0.85f, 0f);
                break;
            case "PowerUp":
                itemColor = Color.cyan;
                break;
            default:
                itemColor = Color.white;
                break;
        }

        itemRenderer.material.color = itemColor;
        if (itemRenderer.material.HasProperty("_EmissionColor"))
        {
            itemRenderer.material.EnableKeyword("_EMISSION");
            itemRenderer.material.SetColor("_EmissionColor", itemColor * 0.5f);
        }
    }
    
    private void RotateItem()
    {
        if (rotationSpeed > 0)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
    
    private void FloatAnimation()
    {
        if (floatSpeed > 0)
        {
            floatTimer += Time.deltaTime;
            float yOffset = Mathf.Sin(floatTimer * floatSpeed) * floatAmplitude;
            transform.position = startPosition + new Vector3(0f, yOffset, 0f);
        }
    }
    
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
    
    public void SetItemType(string type)
    {
        itemType = type;
        SetValueBasedOnType();
        ApplyVisualProperties();
    }
    
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
    
    public void SetValue(string type, int newValue)
    {
        SetItemType(type);
        SetValue(newValue);
    }
    
    public int GetValue()
    {
        return value;
    }
    
    public string GetItemType()
    {
        return itemType;
    }
    
    public string GetItemName()
    {
        return itemName;
    }
    
    public float GetRemainingLifetime()
    {
        if (!despawnsOverTime || lifetime <= 0) return -1f;
        return Mathf.Max(0f, lifetime - (Time.time - spawnTime));
    }
    
    public void Collect()
    {
        if (isCollected) return;
        
        isCollected = true;
        
        if (gameManager != null)
        {
            gameManager.RemoveItem(gameObject);
        }
        
        Debug.Log($"Item collected: {itemName} (Type: {itemType}, Value: {value})");
        
        Destroy(gameObject, 0.1f);
    }
    
    private void Despawn()
    {
        if (gameManager != null)
        {
            gameManager.RemoveItem(gameObject);
        }
        
        Destroy(gameObject);
    }
    
    public bool IsCollectible()
    {
        return !isCollected && gameObject.activeSelf;
    }
    
    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }
    
    public void SetFloatAnimation(float speed, float amplitude)
    {
        floatSpeed = speed;
        floatAmplitude = amplitude;
    }
    
    void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && IsCollectible())
        {
            player.CollectItem(this);
        }
    }
}