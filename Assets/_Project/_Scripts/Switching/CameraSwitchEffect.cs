using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// Subscribes to the global SwitchModes.OnTransitionUpdate and lerps
/// camera zoom + Post Processing Volume weight using the shared t value.
/// No internal coroutine needed � SwitchModes drives everything.
/// </summary>
public class CameraSwitchEffect : MonoBehaviour
{
    [SerializeField] private PostProcessVolume postProcessVolume;
    [SerializeField] private float zoomAmount = 2f;
    [SerializeField] private float ppvTargetWeight = 1f;

    private Camera cam;
    private float baseOrthoSize;

    void Awake()
    {
        cam = GetComponent<Camera>();
        baseOrthoSize = cam.orthographicSize;

        if (postProcessVolume != null)
            postProcessVolume.weight = 0f;
    }

    void OnEnable()  => SwitchModes.OnTransitionUpdate += OnTransitionUpdate;
    void OnDisable() => SwitchModes.OnTransitionUpdate -= OnTransitionUpdate;

    private void OnTransitionUpdate(float t)
    {
        cam.orthographicSize = Mathf.Lerp(baseOrthoSize, baseOrthoSize + zoomAmount, t);

        if (postProcessVolume != null)
            postProcessVolume.weight = Mathf.Lerp(0f, ppvTargetWeight, t);
    }
}
