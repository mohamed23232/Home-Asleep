using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string nextLevel;

    [SerializeField] private ResultUI resultUI;

    public static Action<string> OnGoToNextLevel;

    private InteractSystem interactSystem;

    void Start()
    {
        interactSystem = FindFirstObjectByType<InteractSystem>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CheckStarsAndShowUI();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CheckStarsAndShowUI();
        }
    }


    private void CheckStarsAndShowUI()
    {
        if (interactSystem.CollectedStarCount >= 3)
        {
            resultUI.gameObject.SetActive(true);
            OnGoToNextLevel?.Invoke(nextLevel);
        }
        else
        {
            Debug.Log("Not enough stars");
        }
    }
}
