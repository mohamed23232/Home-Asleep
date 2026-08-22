using UnityEngine;

public class FloatingStar : MonoBehaviour
{
    [SerializeField] private float horizontalAmount = 0.15f;
    [SerializeField] private float verticalAmount = 0.2f;

    [SerializeField] private float horizontalSpeed = 0.7f;
    [SerializeField] private float verticalSpeed = 1f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        float x = Mathf.Sin(Time.time * horizontalSpeed) * horizontalAmount;
        float y = Mathf.Sin(Time.time * verticalSpeed) * verticalAmount;

        transform.localPosition = startPosition + new Vector3(x, y, 0f);
    }
}
