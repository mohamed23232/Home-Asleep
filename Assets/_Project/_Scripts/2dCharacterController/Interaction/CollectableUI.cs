using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectableUI : MonoBehaviour
{

    private InteractSystem interactSystem;

    [SerializeField] private Image[] stars;

    [SerializeField] private TextMeshProUGUI normalCountText;

    void Start()
    {
        interactSystem = FindFirstObjectByType<InteractSystem>();
        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].color = Color.black;
        }
    }

    void Update()
    {
        for (int i = 0; i < interactSystem.CollectedStarCount; i++)
        {
            stars[i].color = Color.white;
        }

        normalCountText.text = interactSystem.CollectedNormalCount.ToString();
    }
}
