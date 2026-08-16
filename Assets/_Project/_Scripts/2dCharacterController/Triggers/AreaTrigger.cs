using UnityEngine;

// Counts objects inside a trigger area to determine active state.
[RequireComponent(typeof(Collider2D))]
public class AreaTrigger : TriggerObject
{
    [Tooltip("Which layers' objects can trigger this")]
    public LayerMask triggerMask;
    [Tooltip("If enabled, will only trigger if a player enters the area")]
    public bool playerOnly;

    public override bool Active { get { return objCount > 0; } }

    private int objCount;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (oneShot && objCount > 0) return;
        if (triggerMask == (triggerMask | (1 << other.gameObject.layer)) &&
            (!playerOnly || other.GetComponent<PlayerController>() != null))
        {
            objCount++;
            animator.SetBool(ANIMATION_ACTIVE, Active);
            RaiseActiveChanged();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (oneShot && objCount > 0) return;
        if (triggerMask == (triggerMask | (1 << other.gameObject.layer)) &&
            (!playerOnly || other.GetComponent<PlayerController>() != null))
        {
            objCount--;
            animator.SetBool(ANIMATION_ACTIVE, Active);
            RaiseActiveChanged();
        }
    }
}