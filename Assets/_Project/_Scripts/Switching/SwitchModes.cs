using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Central transition broadcaster.
/// Runs one shared coroutine and fires OnTransitionUpdate(float t) every frame,
/// where t = 0 means fully Awake and t = 1 means fully Asleep.
/// Any component that needs to animate on switch just subscribes to OnTransitionUpdate.
/// </summary>
public class SwitchModes : MonoBehaviour
{
    [SerializeField] private float transitionDuration = 0.4f;
    [SerializeField] private AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    /// <summary>t = 0 (Awake) .. 1 (Asleep). Fired every frame during transition.</summary>
    public static event Action<float> OnTransitionUpdate;

    /// <summary>Current normalized transition value, accessible without subscribing.</summary>
    public static float CurrentT { get; private set; } = 0f;

    private Coroutine transitionCoroutine;

    void OnEnable()  => PlayerController.OnSwitch += HandleSwitch;
    void OnDisable() => PlayerController.OnSwitch -= HandleSwitch;

    private void HandleSwitch(bool isAwake)
    {
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        float target = isAwake ? 0f : 1f;
        transitionCoroutine = StartCoroutine(Transition(target));
    }

    private IEnumerator Transition(float target)
    {
        float start   = CurrentT;
        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float raw    = Mathf.Clamp01(elapsed / transitionDuration);
            CurrentT     = Mathf.Lerp(start, target, transitionCurve.Evaluate(raw));
            OnTransitionUpdate?.Invoke(CurrentT);
            yield return null;
        }

        CurrentT = target;
        OnTransitionUpdate?.Invoke(CurrentT);
        transitionCoroutine = null;
    }
}
