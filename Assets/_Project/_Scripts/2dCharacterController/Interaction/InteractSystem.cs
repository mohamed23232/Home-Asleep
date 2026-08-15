using UnityEngine;

// Handles collectibles, interactables, and pushable detection near the player.
[RequireComponent(typeof(CharacterController2D))]
public class InteractSystem : MonoBehaviour
{
    [Tooltip("How distant objects can be to be interacted with (visible in a magenta gizmo)")]
    public float interactRange = 0.5f;
    [Tooltip("Offset for the range's starting position relative to the character position")]
    public Vector2 rangePositionOffset;

    public int CollectedCount { get; private set; } = 0;

    // The nearest PushableObject within range, or null if none
    public PushableObject ClosestPushable => closestPushable;

    private InteractableObject closestObject = null;
    private PushableObject closestPushable = null;
    private PhysicsConfig pConfig;

    void Start()
    {
        pConfig = PhysicsConfig.Instance;
        if (!pConfig)
        {
            Debug.LogError("PhysicsConfig not found! Add a PhysicsConfig component to your scene.");
        }
    }

    void Update()
    {
        Vector3 origin = transform.position + (Vector3)rangePositionOffset;

        // Find the closest interactable in range
        closestObject = null;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, interactRange, pConfig.interactableMask);
        foreach (Collider2D hit in hits)
        {
            InteractableObject obj = hit.GetComponent<InteractableObject>();
            if (obj && obj.interactable &&
                (!closestObject || Vector2.Distance(transform.position, closestObject.transform.position) >
                    Vector2.Distance(transform.position, obj.transform.position)))
            {
                closestObject = obj;
            }
        }
        // Auto-collect collectibles on contact
        if (closestObject is CollectibleObject)
        {
            closestObject.Interact(this);
            closestObject = null;
        }

        // Find the closest pushable in range
        closestPushable = null;
        Collider2D[] pushableHits = Physics2D.OverlapCircleAll(origin, interactRange, pConfig.interactableMask);
        foreach (Collider2D hit in pushableHits)
        {
            PushableObject obj = hit.GetComponent<PushableObject>();
            if (obj && (!closestPushable ||
                Vector2.Distance(transform.position, closestPushable.transform.position) >
                Vector2.Distance(transform.position, obj.transform.position)))
            {
                closestPushable = obj;
            }
        }
    }

    public void Interact()
    {
        if (closestObject)
        {
            closestObject.Interact(this);
            closestObject = null;
        }
    }

    public void AddToCount()
    {
        CollectedCount++;
        Debug.Log("Collected " + CollectedCount);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta * new Color(1, 1, 1, 0.5f);
        Gizmos.DrawWireSphere(transform.position + (Vector3)rangePositionOffset, interactRange);
    }
}