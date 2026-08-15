using System;
using UnityEngine;

// Base trigger that can be toggled and notifies listeners when its state changes.
public class TriggerObject : MonoBehaviour
{
    public static readonly string ANIMATION_ACTIVE = "active";

    [Tooltip("If enabled, the trigger will stay active after triggered")]
    public bool oneShot;

    protected Animator animator;

    public virtual bool Active { get; protected set; }

    // Subscribe instead of polling to react to state changes
    public event Action<bool> OnActiveChanged;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void Trigger()
    {
        if (!oneShot || !Active)
        {
            Active = !Active;
            if (animator)
            {
                animator.SetBool(ANIMATION_ACTIVE, Active);
            }
            RaiseActiveChanged();
        }
    }

    // Call from subclasses whenever Active changes outside of Trigger()
    protected void RaiseActiveChanged()
    {
        OnActiveChanged?.Invoke(Active);
    }
}