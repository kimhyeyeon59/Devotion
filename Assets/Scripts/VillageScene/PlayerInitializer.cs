using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    void Awake()
    {
        // PlayerStats 싱글톤은 이미 존재하므로 새로 생성하지 않고 연결만
        // 필요하면 위치 초기화 가능
        if (PlayerStats.Instance != null)
        {
            // 체력 UI 갱신
            PlayerStats.Instance.onHealthChangedCallback?.Invoke();
        }
    }
}
