using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private int TotalObjects = 10;

    [SerializeField] private Button nextLevelBtn;
    [SerializeField] private AudioClip resultSound;

    [SerializeField] private TextMeshProUGUI normalCountText;

    private AudioSource audioSource;
    private InteractSystem interactSystem;

    private string nextLevelIndex;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        LevelManager.OnGoToNextLevel += OnLevelComplete;

        if (audioSource != null && resultSound != null)
        {
            audioSource.PlayOneShot(resultSound);
        }
    }

    private void OnDisable()
    {
        LevelManager.OnGoToNextLevel -= OnLevelComplete;
    }

    private void Start()
    {
        interactSystem = FindFirstObjectByType<InteractSystem>();

        nextLevelBtn.onClick.AddListener(LoadNextLevel);

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            SelectPlayButton();
        }

        normalCountText.text =
            interactSystem.CollectedNormalCount + "/" + TotalObjects;
    }

    private void OnLevelComplete(string nextLevel)
    {
        nextLevelIndex = nextLevel;
    }

    private void LoadNextLevel()
    {
        Debug.Log("Loading next level: " + nextLevelIndex);
        SceneManager.LoadScene(nextLevelIndex);
    }

    private void SelectPlayButton()
    {
        EventSystem.current.SetSelectedGameObject(nextLevelBtn.gameObject);
    }
}