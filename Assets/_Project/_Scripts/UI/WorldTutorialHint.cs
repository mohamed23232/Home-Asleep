using TMPro;
using UnityEngine;

// World-space how-to-play label. Place next to level objects; fades when the player is far.
public class WorldTutorialHint : MonoBehaviour
{
    [SerializeField, TextArea] private string message = "Hint";
    [SerializeField] private Sprite[] keyIcons;
    [SerializeField] private TMP_FontAsset font;
    [SerializeField] private Material fontMaterial;
    [SerializeField] private float fontSize = 3f;
    [SerializeField] private float iconScale = 4.2f;
    [SerializeField] private float iconPadding = 0.35f;
    [SerializeField] private float iconTextGap = 0.2f;
    [SerializeField] private float visibleDistance = 12f;
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private string sortingLayerName = "Frontest";

    private Transform player;
    private TextMeshPro tmp;
    private SpriteRenderer[] iconRenderers;
    private Vector3 restPosition;

    void Awake()
    {
        restPosition = transform.position;

        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;

        BuildLabel();
        BuildIcons();
    }

    void LateUpdate()
    {
        transform.position = restPosition + Vector3.up * (Mathf.Sin(Time.time * 1.7f) * 0.07f);

        float alpha = 1f;
        if (player != null)
        {
            float distance = Vector2.Distance(player.position, restPosition);
            alpha = Mathf.Clamp01(1f - (distance - visibleDistance * 0.45f) / (visibleDistance * 0.55f));
        }

        if (tmp != null)
        {
            Color color = tmp.color;
            color.a = alpha;
            tmp.color = color;
        }

        if (iconRenderers == null)
            return;

        for (int i = 0; i < iconRenderers.Length; i++)
        {
            if (iconRenderers[i] == null)
                continue;
            Color color = iconRenderers[i].color;
            color.a = alpha;
            iconRenderers[i].color = color;
        }
    }

    private void BuildLabel()
    {
        Transform label = new GameObject("Label").transform;
        label.SetParent(transform, false);
        label.localPosition = Vector3.zero;

        tmp = label.gameObject.AddComponent<TextMeshPro>();
        if (font != null)
            tmp.font = font;
        if (fontMaterial != null)
            tmp.fontSharedMaterial = fontMaterial;

        tmp.text = message;
        tmp.fontSize = fontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = textColor;
        tmp.enableWordWrapping = true;
        tmp.overflowMode = TextOverflowModes.Overflow;
        tmp.rectTransform.sizeDelta = new Vector2(Mathf.Max(10f, fontSize * 3.2f), fontSize * 2.6f);
        tmp.outlineWidth = 0.22f;
        tmp.outlineColor = new Color(0.08f, 0.05f, 0.12f, 1f);
        tmp.sortingLayerID = SortingLayer.NameToID(sortingLayerName);
        tmp.sortingOrder = 50;
        tmp.ForceMeshUpdate();
    }

    private void BuildIcons()
    {
        if (keyIcons == null || keyIcons.Length == 0)
            return;
    float textHalfHeight = tmp.bounds.size.y * 0.5f;
        float maxIconHeight = 0f;
        float totalIconWidth = 0f;
        int iconCount = 0;
        for (int i = 0; i < keyIcons.Length; i++)
        {
            if (keyIcons[i] == null)
                continue;
            Vector2 size = SpriteWorldSize(keyIcons[i]);
            maxIconHeight = Mathf.Max(maxIconHeight, size.y);
            totalIconWidth += size.x;
            iconCount++;
        }

        if (iconCount == 0)
            return;

        totalIconWidth += iconPadding * (iconCount - 1);

        iconRenderers = new SpriteRenderer[keyIcons.Length];
        float x = -totalIconWidth * 0.5f;
        float iconY = textHalfHeight + iconTextGap + maxIconHeight * 0.5f;

        for (int i = 0; i < keyIcons.Length; i++)
        {
            if (keyIcons[i] == null)
                continue;

            Vector2 size = SpriteWorldSize(keyIcons[i]);
            x += size.x * 0.5f;

            var icon = new GameObject("KeyIcon");
            icon.transform.SetParent(transform, false);
            icon.transform.localPosition = new Vector3(x, iconY, 0f);
            icon.transform.localScale = Vector3.one * iconScale;

            var renderer = icon.AddComponent<SpriteRenderer>();
            renderer.sprite = keyIcons[i];
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = 51;
            iconRenderers[i] = renderer;

            x += size.x * 0.5f + iconPadding;
        }
    }

    private Vector2 SpriteWorldSize(Sprite sprite)
    {
        return sprite.bounds.size * iconScale;
    }

}
