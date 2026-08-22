using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalLevelManager : MonoBehaviour
{
    [SerializeField] private string creditsSceneName;   // name of your credits scene
    [SerializeField] private Animator cutsceneAnimator; // Animator that plays your cutscene
    [SerializeField] private GameObject allUIParent;    // parent object holding all gameplay UI

    private InteractSystem interactSystem;
    private bool cutsceneStarted = false;

    void Start()
    {
        interactSystem = FindFirstObjectByType<InteractSystem>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.root.CompareTag("Player") && !cutsceneStarted)
        {
            if (interactSystem == null)
            {
                Debug.LogError("InteractSystem not found!");
                return;
            }
            if (cutsceneAnimator == null)
            {
                Debug.LogError("Cutscene Animator not assigned!");
                return;
            }

            if (interactSystem.CollectedStarCount >= 3)
            {
                cutsceneStarted = true;

                // Disable all gameplay UI
                if (allUIParent != null)
                    allUIParent.SetActive(false);

                // Enable cutscene canvas
                cutsceneAnimator.gameObject.SetActive(true);

                // Play cutscene
                cutsceneAnimator.Play("FScene"); // must match Animator state name
            }
            else
            {
                Debug.Log("Not enough stars");
            }
        }
    }

    // Called at the end of the cutscene via Animation Event
    public void OnCutsceneFinished()
    {
        SceneManager.LoadScene(creditsSceneName);
    }
}
