using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int nextLevel = 1;

    private InteractSystem interactSystem;

    void Start()
    {
        interactSystem = FindObjectOfType<InteractSystem>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && interactSystem.CollectedStarCount == 3)
            SceneManager.LoadScene(nextLevel);
        else
            Debug.Log("Not enough stars");
    }
}
