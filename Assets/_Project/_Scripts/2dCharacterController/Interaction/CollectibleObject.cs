using UnityEngine;

// Auto-collected when the player enters range. Increments the count and destroys itself.
[RequireComponent(typeof(Collider2D))]
public class CollectibleObject : InteractableObject
{
    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    public override void Interact(InteractSystem interactSystem)
    {
        interactSystem.AddToCount();
        Destroy(gameObject);
    }
}
