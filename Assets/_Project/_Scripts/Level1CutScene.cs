using UnityEngine;

public class Level1CutScene : MonoBehaviour
{
    [Header("References")]
    public GameObject cutscenePanel;   // UI panel or canvas with your PNG animation
    public GameObject gameplayObjects; // Parent object holding gameplay elements

    private bool cutscenePlaying = true;

    void Start()
    {
        // Disable gameplay at the start
        gameplayObjects.SetActive(false);
        cutscenePanel.SetActive(true);
    }

    // This method should be called at the end of the cutscene animation
    public void OnCutsceneFinished()
    {
        cutscenePanel.SetActive(false);
        gameplayObjects.SetActive(true);
        cutscenePlaying = false;
    }

    void Update()
    {
        // Optional: allow skipping with Space or Esc
        if (cutscenePlaying && Input.GetKeyDown(KeyCode.Space))
        {
            OnCutsceneFinished();
        }
    }
}
