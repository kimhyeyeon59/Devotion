using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public static HealthBarController Instance { get; private set; }

    private GameObject[] heartContainers;
    private Image[] heartFills;

    public Transform heartsParent;
    public GameObject heartContainerPrefab;

    private PlayerStats playerStats;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (heartsParent != null)
            DontDestroyOnLoad(heartsParent.gameObject); // 프리팹 UI 유지
    }

    private void Start()
    {
        playerStats = PlayerStats.Instance;

        if (playerStats == null) return;

        heartContainers = new GameObject[(int)playerStats.MaxTotalHealth];
        heartFills = new Image[(int)playerStats.MaxTotalHealth];

        playerStats.onHealthChangedCallback += UpdateHeartsHUD;

        InstantiateHeartContainers();
        UpdateHeartsHUD();
    }

    public void UpdateHeartsHUD()
    {
        SetHeartContainers();
        SetFilledHearts();
    }

    void SetHeartContainers()
    {
        if (heartContainers == null || playerStats == null) return;

        for (int i = 0; i < heartContainers.Length; i++)
        {
            if (heartContainers[i] != null)
                heartContainers[i].SetActive(i < playerStats.MaxHealth);
        }
    }

    void SetFilledHearts()
    {
        if (heartFills == null || playerStats == null) return;

        for (int i = 0; i < heartFills.Length; i++)
        {
            if (heartFills[i] != null)
                heartFills[i].fillAmount = (i < Mathf.FloorToInt(playerStats.Health)) ? 1f : 0f;
        }

        if (playerStats.Health % 1 != 0)
        {
            int lastPos = Mathf.FloorToInt(playerStats.Health);
            if (lastPos < heartFills.Length && heartFills[lastPos] != null)
                heartFills[lastPos].fillAmount = playerStats.Health % 1;
        }
    }

    void InstantiateHeartContainers()
    {
        if (playerStats == null || heartsParent == null || heartContainerPrefab == null) return;

        for (int i = 0; i < playerStats.MaxTotalHealth; i++)
        {
            GameObject temp = Instantiate(heartContainerPrefab);
            temp.transform.SetParent(heartsParent, false);
            heartContainers[i] = temp;

            Transform fill = temp.transform.Find("HeartFill");
            if (fill != null)
                heartFills[i] = fill.GetComponent<Image>();
        }
    }
}
