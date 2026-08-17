using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelectionVisual : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private Image targetImage;
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Sprite normalSprite;

    private void Awake()
    {
        targetImage.sprite = normalSprite;
    }

    public void OnSelect(BaseEventData eventData)
    {
        targetImage.sprite = selectedSprite;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        targetImage.sprite = normalSprite;
    }
}