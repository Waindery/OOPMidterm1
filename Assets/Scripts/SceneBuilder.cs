using UnityEngine;

[DefaultExecutionOrder(-100)]
public class SceneBuilder : MonoBehaviour
{
    [Header("Arena Settings")]
    public Vector2 arenaSize = new Vector2(20f, 20f);
    public float wallHeight = 3f;
    public float wallThickness = 0.5f;

    [Header("Visuals")]
    public Color groundColor = new Color(0.25f, 0.25f, 0.25f);
    public Color wallColor = new Color(0.15f, 0.15f, 0.15f);
    public Material overrideMaterial;

    [Header("Camera")]
    public Vector3 cameraOffset = new Vector3(0f, 15f, -15f);
    public Vector3 cameraAngles = new Vector3(35f, 0f, 0f);

    private const string GroundName = "Ground";
    private const string WallParentName = "ArenaWalls";

    private void Awake()
    {
        BuildGround();
        BuildWalls();
        ConfigureCamera();
        EnsureDirectionalLight();
    }

    private void BuildGround()
    {
        GameObject ground = GameObject.Find(GroundName);

        if (ground == null)
        {
            ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = GroundName;
        }

        ground.transform.position = Vector3.zero;
        ground.transform.localScale = new Vector3(arenaSize.x / 10f, 1f, arenaSize.y / 10f);
        ground.tag = "Ground";

        GroundPlane groundPlane = ground.GetComponent<GroundPlane>();
        if (groundPlane == null)
        {
            groundPlane = ground.AddComponent<GroundPlane>();
        }
        groundPlane.groundHeight = 0f;
        groundPlane.groundSize = new Vector3(arenaSize.x, 0.5f, arenaSize.y);

        ApplyColor(ground, groundColor);
    }

    private void BuildWalls()
    {
        GameObject wallsParent = GameObject.Find(WallParentName);
        if (wallsParent == null)
        {
            wallsParent = new GameObject(WallParentName);
        }

        Vector3 halfSize = new Vector3(arenaSize.x * 0.5f, wallHeight * 0.5f, arenaSize.y * 0.5f);

        CreateWall("NorthWall", new Vector3(0f, halfSize.y, halfSize.z + wallThickness * 0.5f),
            new Vector3(arenaSize.x, wallHeight, wallThickness), wallsParent.transform);
        CreateWall("SouthWall", new Vector3(0f, halfSize.y, -halfSize.z - wallThickness * 0.5f),
            new Vector3(arenaSize.x, wallHeight, wallThickness), wallsParent.transform);
        CreateWall("EastWall", new Vector3(halfSize.x + wallThickness * 0.5f, halfSize.y, 0f),
            new Vector3(wallThickness, wallHeight, arenaSize.y), wallsParent.transform);
        CreateWall("WestWall", new Vector3(-halfSize.x - wallThickness * 0.5f, halfSize.y, 0f),
            new Vector3(wallThickness, wallHeight, arenaSize.y), wallsParent.transform);
    }

    private void CreateWall(string name, Vector3 position, Vector3 size, Transform parent)
    {
        GameObject wall = GameObject.Find(name);
        if (wall == null)
        {
            wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wall.name = name;
        }

        wall.transform.SetParent(parent);
        wall.transform.position = position;
        wall.transform.localScale = size;

        Collider collider = wall.GetComponent<Collider>();
        if (collider == null)
        {
            collider = wall.AddComponent<BoxCollider>();
        }
        collider.isTrigger = false;

        ApplyColor(wall, wallColor);
    }

    private void ConfigureCamera()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            GameObject camObj = new GameObject("Main Camera");
            cam = camObj.AddComponent<Camera>();
            cam.tag = "MainCamera";
        }

        cam.transform.position = cameraOffset;
        cam.transform.rotation = Quaternion.Euler(cameraAngles);

        if (cam.orthographic)
        {
            cam.orthographic = false;
        }
    }

    private void EnsureDirectionalLight()
    {
        Light light = FindObjectOfType<Light>();
        if (light == null)
        {
            GameObject lightObj = new GameObject("Directional Light");
            light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional;
            light.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }
    }

    private void ApplyColor(GameObject target, Color color)
    {
        Renderer renderer = target.GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = target.GetComponentInChildren<Renderer>();
        }

        if (renderer != null)
        {
            if (overrideMaterial != null)
            {
                renderer.sharedMaterial = overrideMaterial;
            }
            renderer.sharedMaterial.color = color;
        }
    }
}

