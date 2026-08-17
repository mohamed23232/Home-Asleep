using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CollectibleObject : InteractableObject
{
    private const float SOUND_VOLUME = 0.3f;

    enum CollectibleType
    {
        Star,
        normal,
    }

    [Header("Floating Animation")]
    [SerializeField] private bool enableFloating = true;
    [SerializeField] private float floatAmplitude = 0.2f;
    [SerializeField] private float floatSpeed = 2f;

    [SerializeField] private CollectibleType type = CollectibleType.normal;

    private float startY;
    private float timeOffset;

    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void Start()
    {
        startY = transform.localPosition.y;
        timeOffset = transform.position.x;
    }

    void Update()
    {
        if (enableFloating)
        {
            float newY = startY + Mathf.Sin((Time.time + timeOffset) * floatSpeed) * floatAmplitude;
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
    }

    [Header("Audio")]
    [SerializeField] private AudioClip collectSound;

    public override void Interact(InteractSystem interactSystem)
    {
        if (type == CollectibleType.Star)
        {
            interactSystem.AddToStarCount();
        }
        else
        {
            interactSystem.AddToNormalCount();
        }

        if (collectSound != null)
        {
            // Play the sound at the camera's position or the object's position
            // We use the camera position so the sound is consistently loud since the object will be destroyed
            if (Camera.main != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, Camera.main.transform.position, SOUND_VOLUME);
            }
            else
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position, SOUND_VOLUME);
            }
        }

        Destroy(gameObject);
    }
}
