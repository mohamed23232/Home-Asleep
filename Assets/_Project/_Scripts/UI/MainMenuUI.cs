using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private string firstLevelName;
    [SerializeField] private string creditsSceneName;


    private void OnEnable()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        creditsButton.onClick.AddListener(OnCreditsClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(OnPlayClicked);
        creditsButton.onClick.RemoveListener(OnCreditsClicked);
        quitButton.onClick.RemoveListener(OnQuitClicked);
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
        SceneManager.LoadScene(firstLevelName);
    }

    private void OnCreditsClicked()
    {
        SceneManager.LoadScene(creditsSceneName);
    }

    private void OnQuitClicked()
    {
        Application.Quit();
    }
}
