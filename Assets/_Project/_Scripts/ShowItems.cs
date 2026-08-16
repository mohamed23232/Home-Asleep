using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShowItems : MonoBehaviour
{
    [SerializeField] private float fadedAlpha = 0.3f;

    private int pressCount = 0;
    private readonly List<GameObject> asleepObjects = new();
    private readonly Dictionary<SpriteRenderer, Color> originalColors = new();

    public InputMaster input;

    void Awake()
    {
        input = new InputMaster();
        input.Player.Switch.performed += OnSwitch;
    }

    void Start()
    {
        asleepObjects.AddRange(GameObject.FindGameObjectsWithTag("Asleep_platform"));

        foreach (GameObject obj in asleepObjects)
        {
            foreach (SpriteRenderer sr in obj.GetComponentsInChildren<SpriteRenderer>(true))
            {
                originalColors[sr] = sr.color;
            }
        }

        SetAsleepState(false);
    }

    void OnEnable()  => input.Player.Switch.Enable();
    void OnDisable() => input.Player.Switch.Disable();

    void OnDestroy()
    {
        input.Player.Switch.performed -= OnSwitch;
    }

    private void OnSwitch(InputAction.CallbackContext ctx)
    {
        pressCount++;
        bool isAsleep = pressCount % 2 == 1;
        SetAsleepState(isAsleep);
    }

    private void SetAsleepState(bool isAsleep)
    {
        foreach (GameObject obj in asleepObjects)
        {
            foreach (SpriteRenderer sr in obj.GetComponentsInChildren<SpriteRenderer>(true))
            {
                if (originalColors.TryGetValue(sr, out Color original))
                {
                    Color c = original;
                    c.a = isAsleep ? original.a : fadedAlpha;
                    sr.color = c;
                }
            }

            if (obj.TryGetComponent<Collider2D>(out var col))
            {
                col.enabled = isAsleep;
            }
        }
    }
}
