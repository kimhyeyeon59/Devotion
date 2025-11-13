using UnityEngine;

public class SignTrigger : MonoBehaviour
{
    public GameObject infoObject; // 안내 메시지 오브젝트

    private void Start()
    {
        if (infoObject != null)
            infoObject.SetActive(false); // 시작 시 꺼져 있어야 함
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (infoObject != null)
                infoObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (infoObject != null)
                infoObject.SetActive(false);
        }
    }
}
