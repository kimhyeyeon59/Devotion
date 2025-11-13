using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;

    public delegate void OnPotionCountChangedDelegate();
    public OnPotionCountChangedDelegate onPotionCountChangedCallback;

    #region Singleton
    private static PlayerStats instance;
    public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerStats>();
            return instance;
        }
        private set { instance = value; }
    }
    #endregion

    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxTotalHealth;

    public float Health { get { return health; } }
    public float MaxHealth { get { return maxHealth; } }
    public float MaxTotalHealth { get { return maxTotalHealth; } }

    [SerializeField] private int potionCount = 0;
    public int PotionCount { get { return potionCount; } }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddPotion(int amount)
    {
        potionCount += amount;
        onPotionCountChangedCallback?.Invoke();
    }

    public bool UsePotion()
    {
        if (potionCount > 0 && health < maxHealth)
        {
            potionCount--;
            Heal(1);
            onPotionCountChangedCallback?.Invoke();
            return true;
        }
        return false;
    }

    public void Heal(float amount)
    {
        health += amount;
        ClampHealth();
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        ClampHealth();
    }

    public void AddHealth()
    {
        if (maxHealth < maxTotalHealth)
        {
            maxHealth += 1;
            health = maxHealth;
            onHealthChangedCallback?.Invoke();
        }
    }

    private void ClampHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        onHealthChangedCallback?.Invoke();
    }
}
