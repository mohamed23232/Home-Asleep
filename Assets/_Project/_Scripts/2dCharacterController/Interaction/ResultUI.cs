using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultUI: MonoBehaviour
{
    [SerializeField] private int TotalObjects = 10;

    [SerializeField] private Button nextLevelBtn;
    
    private InteractSystem interactSystem;
    
    [SerializeField] private TextMeshProUGUI normalCountText;
    

    void OnEnable()
    {
        LevelManager.OnGoToNextLevel += NextLevel;
    }

    void OnDisable()
    {
        LevelManager.OnGoToNextLevel -= NextLevel;
    }

    void Start()
    {
        interactSystem = FindObjectOfType<InteractSystem>();
        nextLevelBtn.onClick.AddListener(NextLevel);
        gameObject.SetActive(false);
    }

    void Update()
    {
        normalCountText.text = interactSystem.CollectedNormalCount.ToString() + "/" + TotalObjects;
    }

    void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}