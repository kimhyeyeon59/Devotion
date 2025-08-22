using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private GameObject[] heartContainers;
    private Image[] heartFills;

    public Transform heartsParent;            // Canvas 안 하트 부모
    public GameObject heartContainerPrefab;   // 하트 프리팹

    private PlayerStats playerStats;          // 씬에 배치한 PlayerStats 참조

    private void Start()
    {
        playerStats = PlayerStats.Instance;

        heartContainers = new GameObject[(int)playerStats.MaxTotalHealth];
        heartFills = new Image[(int)playerStats.MaxTotalHealth];

        // 체력 변경 시 UI 업데이트
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
        for (int i = 0; i < heartContainers.Length; i++)
        {
            heartContainers[i].SetActive(i < playerStats.MaxHealth);
        }
    }

    void SetFilledHearts()
    {
        for (int i = 0; i < heartFills.Length; i++)
        {
            heartFills[i].fillAmount = (i < Mathf.FloorToInt(playerStats.Health)) ? 1f : 0f;
        }

        if (playerStats.Health % 1 != 0)
        {
            int lastPos = Mathf.FloorToInt(playerStats.Health);
            if (lastPos < heartFills.Length)
                heartFills[lastPos].fillAmount = playerStats.Health % 1;
        }
    }

    void InstantiateHeartContainers()
    {
        for (int i = 0; i < playerStats.MaxTotalHealth; i++)
        {
            GameObject temp = Instantiate(heartContainerPrefab);
            temp.transform.SetParent(heartsParent, false);
            heartContainers[i] = temp;
            heartFills[i] = temp.transform.Find("HeartFill").GetComponent<Image>();
        }
    }
}
