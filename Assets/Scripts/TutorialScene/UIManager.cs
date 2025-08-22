using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI potionCountText;
    private int potionCount = 0;

    public AudioClip potionDrinkClip;
    private AudioSource audioSource;

    public GameObject potionHintUI;
    public GameObject attackHintObject;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdatePotionText();
    }

    public void AddPotion(int amount)
    {
        potionCount += amount;
        UpdatePotionText();
    }

    public void UsePotion()
    {
        if (potionCount > 0 && PlayerStats.Instance.Health < PlayerStats.Instance.MaxHealth)
        {
            potionCount--;
            PlayerStats.Instance.Heal(1);
            UpdatePotionText();

            // 효과음 재생
            if (audioSource != null && potionDrinkClip != null)
                audioSource.PlayOneShot(potionDrinkClip);
        }
    }

    void UpdatePotionText()
    {
        if (potionCountText != null)
            potionCountText.text = "x" + potionCount.ToString();
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
}
