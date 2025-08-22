using UnityEngine;

public class PlayerHealthInitializer : MonoBehaviour
{
    void Start()
    {
        // PlayerStats에 연결된 하트 UI를 강제로 한 번 갱신
        PlayerStats.Instance.onHealthChangedCallback?.Invoke();
    }
}
