using UnityEngine;
using UnityEngine.Serialization;

public class VisibilityController : MonoBehaviour
{
    [SerializeField] private Collider2D[] objs;

    [FormerlySerializedAs("visiableInAwake")]
    [SerializeField] private bool visibleInAwake = false;

    void OnEnable() => PlayerController.OnSwitch += HandleSwitch;
    void OnDisable() => PlayerController.OnSwitch -= HandleSwitch;

    void Start()
    {
        HandleSwitch(SwitchModes.CurrentT < 0.5f);
    }

    private void HandleSwitch(bool isAwake)
    {
        bool enable = isAwake == visibleInAwake;
        foreach (var obj in objs)
            obj.enabled = enable;
    }
}

