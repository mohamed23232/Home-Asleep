using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private int TotalObjects = 10;

    [SerializeField] private Button nextLevelBtn;

    private InteractSystem interactSystem;

    [SerializeField] private TextMeshProUGUI normalCountText;

    private string nextLevelIndex;

    void OnEnable()
    {
        LevelManager.OnGoToNextLevel += OnLevelComplete;
    }

    void OnDisable()
    {
        LevelManager.OnGoToNextLevel -= OnLevelComplete;
    }



    private void Start()
    {
        SelectPlayButton();
        interactSystem = FindFirstObjectByType<InteractSystem>();
        nextLevelBtn.onClick.AddListener(LoadNextLevel);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            SelectPlayButton();
        }
        normalCountText.text = interactSystem.CollectedNormalCount.ToString() + "/" + TotalObjects;
    }

    void OnLevelComplete(string nextLevel)
    {
        nextLevelIndex = nextLevel;
    }

    void LoadNextLevel()
    {
        Debug.Log("Loading next level: " + nextLevelIndex);
        SceneManager.LoadScene(nextLevelIndex);
    }

    private void SelectPlayButton()
    {
        EventSystem.current.SetSelectedGameObject(nextLevelBtn.gameObject);
    }
}