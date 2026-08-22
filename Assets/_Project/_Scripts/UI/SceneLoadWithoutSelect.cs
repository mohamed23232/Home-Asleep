using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadWithoutSelect : MonoBehaviour
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

    private void OnPlayClicked()
    {
        LoadScene();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}