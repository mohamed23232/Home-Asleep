using UnityEngine;

public class VisiabilityController : MonoBehaviour
{
    [SerializeField] private GameObject obj;

    [SerializeField] private bool visiableInAwake = false;

    void OnEnable()  => SwitchModes.OnTransitionUpdate += OnTransitionUpdate;
    void OnDisable() => SwitchModes.OnTransitionUpdate -= OnTransitionUpdate;

    void Start()
    {
        OnTransitionUpdate(SwitchModes.CurrentT);
    }

    private void OnTransitionUpdate(float t)
    {
        // t = 0 is Awake, t = 1 is Asleep
        if (visiableInAwake)
        {
            // Visible during Awake, Invisible during Asleep
            obj.SetActive(t < 0.5f);
        }
        else
        {
            // Invisible during Awake, Visible during Asleep
            obj.SetActive(t > 0.5f);
        }
    }
}

