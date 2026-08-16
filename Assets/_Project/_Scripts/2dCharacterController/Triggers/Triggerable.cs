using UnityEngine;

// Mirrors a TriggerObject's active state to this object's Animator via event — no polling.
public class Triggerable : MonoBehaviour
{
    public TriggerObject trigger;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (trigger)
        {
            trigger.OnActiveChanged += OnTriggerActiveChanged;
        }
    }

    void OnDestroy()
    {
        if (trigger)
        {
            trigger.OnActiveChanged -= OnTriggerActiveChanged;
        }
    }

    private void OnTriggerActiveChanged(bool active)
    {
        if (animator)
        {
            animator.SetBool(TriggerObject.ANIMATION_ACTIVE, active);
        }
    }
}