using UnityEngine;

// Scene-wide physics configuration. Access via PhysicsConfig.Instance.
public class PhysicsConfig : MonoBehaviour
{
    public static PhysicsConfig Instance { get; private set; }

    [Tooltip("Which layers are considered ground")]
    public LayerMask groundMask;
    [Tooltip("Which layers are considered one way platforms")]
    public LayerMask owPlatformMask;
    [Tooltip("Which layers characters can collide with")]
    public LayerMask characterCollisionMask;
    [Tooltip("Which layers stand-on objects will move")]
    public LayerMask standOnCollisionMask;
    [Tooltip("Which layers are considered interactable objects")]
    public LayerMask interactableMask;

    public float gravity = -30f;
    public float airFriction = 15f;
    public float groundFriction = 30f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        ApplyDefaults();
    }

    private void ApplyDefaults()
    {
        if (groundMask == 0)            groundMask            = LayerMask.GetMask("Ground");
        if (owPlatformMask == 0)        owPlatformMask        = LayerMask.GetMask("OWPlatform");
        if (characterCollisionMask == 0) characterCollisionMask = LayerMask.GetMask("Ground");
        if (standOnCollisionMask == 0)  standOnCollisionMask  = LayerMask.GetMask("Character", "Box");
        if (interactableMask == 0)      interactableMask      = LayerMask.GetMask("Box");
    }
}