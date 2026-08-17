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
        interactSystem = FindObjectOfType<InteractSystem>();

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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
}
