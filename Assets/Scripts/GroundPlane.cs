using UnityEngine;

[ExecuteAlways]
public class GroundPlane : MonoBehaviour
{
    [Header("Ground Settings")]
    [Tooltip("Y position of the playable ground surface.")]
    public float groundHeight = 0f;

    [Tooltip("Size of the ground collider.")]
    public Vector3 groundSize = new Vector3(20f, 0.5f, 20f);

    [Tooltip("Automatically keep the collider enabled.")]
    public bool ensureColliderEnabled = true;

    [Tooltip("Snap the plane to the ground height on play.")]
    public bool snapOnStart = true;

    private BoxCollider groundCollider;

    private void Awake()
    {
        SetupGround();
    }

    private void OnValidate()
    {
        SetupGround();
    }

    private void SetupGround()
    {
        if (snapOnStart)
        {
            Vector3 pos = transform.position;
            pos.y = groundHeight;
            transform.position = pos;
        }

        groundCollider = GetComponent<BoxCollider>();
        if (groundCollider == null)
        {
            groundCollider = gameObject.AddComponent<BoxCollider>();
        }

        groundCollider.size = groundSize;
        groundCollider.center = Vector3.zero;
        groundCollider.enabled = ensureColliderEnabled;
    }
}

