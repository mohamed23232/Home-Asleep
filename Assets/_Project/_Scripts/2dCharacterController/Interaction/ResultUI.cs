using TMPro;
using UnityEngine;
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

    void Start()
    {
        interactSystem = FindFirstObjectByType<InteractSystem>();
        nextLevelBtn.onClick.AddListener(LoadNextLevel);
        gameObject.SetActive(false);
    }

    void Update()
    {
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
}