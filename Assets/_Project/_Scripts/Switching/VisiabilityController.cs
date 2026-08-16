using UnityEngine;
using UnityEngine.Serialization;

public class VisibilityController : MonoBehaviour
{
    [SerializeField] private Collider2D[] objs;

    [FormerlySerializedAs("visiableInAwake")]
    [SerializeField] private bool visibleInAwake = false;

    void OnEnable()  => SwitchModes.OnTransitionUpdate += OnTransitionUpdate;
    void OnDisable() => SwitchModes.OnTransitionUpdate -= OnTransitionUpdate;

    void Start()
    {
        OnTransitionUpdate(SwitchModes.CurrentT);
    }

    private void OnTransitionUpdate(float t)
    {
        // t = 0 is Awake, t = 1 is Asleep
        bool enable = visibleInAwake ? (t < 0.5f) : (t > 0.5f);
        foreach (var obj in objs)
            obj.enabled = enable;
    }
}

