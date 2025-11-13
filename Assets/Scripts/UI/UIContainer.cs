using UnityEngine;

public class UIContainer : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // 자식 포함 전체 유지
    }
}
