using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip normalBGM;

    private AudioSource audioSource;

    private void Awake()
    {
        // Prevent duplicates if we return to the Main Menu
        BackgroundMusic existingMusic = FindFirstObjectByType<BackgroundMusic>();

        if (existingMusic != null && existingMusic != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = normalBGM;
            audioSource.Play();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Keep music in Main Menu and Credits
        if (scene.name == "MainMenu" || scene.name == "Credits")
        {
            return;
        }

        // Stop and destroy music when entering a level
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}