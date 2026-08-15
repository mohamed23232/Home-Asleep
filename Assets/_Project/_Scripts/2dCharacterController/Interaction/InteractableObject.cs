using UnityEngine;

// Base class for objects the player can interact with.
public abstract class InteractableObject : MonoBehaviour
{
    public bool interactable = true;
    public abstract void Interact(InteractSystem interactSystem);
}