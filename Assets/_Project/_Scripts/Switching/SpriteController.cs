using UnityEngine;

public class SpriteController : MonoBehaviour
{
    [SerializeField] private Color awakeColor;
    [SerializeField] private Color asleepColor;

    private SpriteRenderer[] spriteRenderers;

    void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer sr in spriteRenderers)
            sr.color = awakeColor;
    }

    void OnEnable()  => SwitchModes.OnTransitionUpdate += OnTransitionUpdate;
    void OnDisable() => SwitchModes.OnTransitionUpdate -= OnTransitionUpdate;

    private void OnTransitionUpdate(float t)
    {
        Color color = Color.Lerp(awakeColor, asleepColor, t);
        foreach (SpriteRenderer sr in spriteRenderers)
            sr.color = color;
    }
}
