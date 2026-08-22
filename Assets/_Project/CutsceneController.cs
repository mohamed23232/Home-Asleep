using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    [Header("References")]
    public Animator cutsceneAnimator;   // Animator with your cutscene animation
    public string nextSceneName;        // Name of the scene to load after cutscene

    void Start()
    {
        // Play the cutscene animation immediately when entering the scene
        cutsceneAnimator.Play("FinalCutscene");
    }

    // This function will be called at the END of the animation
    public void OnCutsceneFinished()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
