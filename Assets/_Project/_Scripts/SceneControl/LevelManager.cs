using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int nextLevel = 1;

    [SerializeField] private ResultUI resultUI;

    public static Action OnGoToNextLevel;

    private InteractSystem interactSystem;

    void Start()
    {
        interactSystem = FindObjectOfType<InteractSystem>();
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && interactSystem.CollectedStarCount >= 3)
        {
            OnGoToNextLevel?.Invoke();
            resultUI.gameObject.SetActive(true);
        }
        else
            Debug.Log("Not enough stars");
    }
}
