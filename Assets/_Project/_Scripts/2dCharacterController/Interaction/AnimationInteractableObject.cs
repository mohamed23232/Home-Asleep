using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public class AnimationInteractableObject : InteractableObject
{
    [SerializeField] private string animationBool;

    public UnityEvent onInteract;

    private Animator animator;
    private Collider2D coll;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        coll.isTrigger = true;
    }

    public override void Interact(InteractSystem interactSystem)
    {
        Debug.Log("Interacted with " + gameObject.name);
        coll.enabled = false;

        if (!string.IsNullOrEmpty(animationBool))
            animator.SetBool(animationBool, true);

        onInteract?.Invoke();

        Invoke(nameof(ReenableCollider), 0.5f);
    }

    private void ReenableCollider()
    {
        coll.enabled = true;
        if (!string.IsNullOrEmpty(animationBool))
            animator.SetBool(animationBool, false);
    }
}