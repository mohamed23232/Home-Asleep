using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoad : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Button playButton;

    private void OnEnable()
    {
        playButton.onClick.AddListener(OnPlayClicked);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(OnPlayClicked);
    }

    private void Start()
    {
        SelectPlayButton();
    }

    private void Update()
    {
        // If no UI element is selected, select Play again
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            SelectPlayButton();
        }
    }

    private void SelectPlayButton()
    {
        EventSystem.current.SetSelectedGameObject(playButton.gameObject);
    }

    private void OnPlayClicked()
    {
        LoadScene();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}