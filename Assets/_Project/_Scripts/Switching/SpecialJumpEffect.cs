using System.Collections;
using UnityEngine;

// Attach to the same GameObject as CameraSwitchEffect (the Main Camera).
// Listens for PlayerController.OnSpecialJump and applies slow-motion + camera zoom.
public class SpecialJumpEffect : MonoBehaviour
{
    [SerializeField] private float slowMotionScale = 0.25f;
    [SerializeField] private float holdDuration = 1.2f;   // unscaled seconds at peak
    [SerializeField] private float rampDuration = 0.15f;  // unscaled seconds to blend in/out
    [SerializeField] private float zoomIn = 2f;           // units subtracted from orthographic size

    private Camera cam;
    private float baseOrthoSize;
    private Coroutine effectCoroutine;

    void Awake()
    {
        cam = GetComponent<Camera>();
        baseOrthoSize = cam.orthographicSize;
    }

    void OnEnable()  => PlayerController.OnSpecialJump += Trigger;
    void OnDisable() => PlayerController.OnSpecialJump -= Trigger;

    private void Trigger()
    {
        if (effectCoroutine != null)
            StopCoroutine(effectCoroutine);
        effectCoroutine = StartCoroutine(RunEffect());
    }

    private IEnumerator RunEffect()
    {
        float targetOrtho = baseOrthoSize - zoomIn;

        // Ramp in
        yield return Ramp(Time.timeScale, slowMotionScale, cam.orthographicSize, targetOrtho, rampDuration);

        // Hold
        float elapsed = 0f;
        while (elapsed < holdDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        // Ramp out
        yield return Ramp(Time.timeScale, 1f, cam.orthographicSize, baseOrthoSize, rampDuration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        cam.orthographicSize = baseOrthoSize;
        effectCoroutine = null;
    }

    private IEnumerator Ramp(float fromTS, float toTS, float fromOrtho, float toOrtho, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            Time.timeScale = Mathf.Lerp(fromTS, toTS, t);
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            cam.orthographicSize = Mathf.Lerp(fromOrtho, toOrtho, t);
            yield return null;
        }
    }

    void OnDestroy()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
}
