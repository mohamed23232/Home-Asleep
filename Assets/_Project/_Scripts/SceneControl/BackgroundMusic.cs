using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip uiBGM;
    [SerializeField] private AudioClip levelBGM;

    private AudioSource audioSource;

    private void Awake()
    {
        // Prevent duplicates
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
        PlayMusicForScene(SceneManager.GetActiveScene());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene);
    }

    private void PlayMusicForScene(Scene scene)
    {
        bool isUI = scene.name == "MainMenu" || scene.name == "Credits";

        AudioClip clip = isUI ? uiBGM : levelBGM;

        // Don't restart the music if we're already playing the correct clip
        if (audioSource.clip == clip && audioSource.isPlaying)
            return;

        audioSource.clip = clip;
        audioSource.Play();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}