using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TextMeshProUGUI potionCountText;

    public AudioClip potionDrinkClip;
    private AudioSource audioSource;

    public GameObject potionHintUI;
    public GameObject attackHintObject;

    public GameObject HideUI; // 숨기고 싶은 UI

    // 숨기고 싶은 씬 이름 배열
    private string[] hideUIScenes = { "LoadingScene", "LoadingScene2", "BlackLoadingScene" };

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 이미 존재하면 중복 제거
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdatePotionText();
    }

    void UpdatePotionText()
    {
        if (potionCountText != null && PlayerStats.Instance != null)
            potionCountText.text = "x" + PlayerStats.Instance.PotionCount.ToString();
    }


    public void AddPotion(int amount)
    {
        PlayerStats.Instance.AddPotion(amount);
        UpdatePotionText();
    }

    public void UsePotion()
    {
        if (PlayerStats.Instance.UsePotion())
        {
            UpdatePotionText();
            if (audioSource != null && potionDrinkClip != null)
                audioSource.PlayOneShot(potionDrinkClip);
        }
    }

    public void ShowPotionHint()
    {
        if (potionHintUI != null)
            potionHintUI.SetActive(true);
    }

    public void ShowAttackHint()
    {
        if (attackHintObject != null)
            attackHintObject.SetActive(true);
    }

    public void HideAttackHint()
    {
        if (attackHintObject != null)
            attackHintObject.SetActive(false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool hideUI = false;

        // 배열에 현재 씬 이름이 있는지 체크
        foreach (var sceneName in hideUIScenes)
        {
            if (scene.name == sceneName)
            {
                hideUI = true;
                break;
            }
        }

        // UI 활성/비활성 처리
        if (HideUI != null)
            HideUI.SetActive(!hideUI);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
