using UnityEngine;

public class InteractionHint : MonoBehaviour
{
    public GameObject hintUIObject; // 조작키 문구 UI
    private bool isPlayerInRange = false;
    private bool hasTalked = false;

    void Start()
    {
        if (hintUIObject != null)
        {
            hintUIObject.SetActive(false); // 처음엔 꺼져 있음
        }
    }

    void Update()
    {
        // ALT 키로 상호작용 시도
        if (isPlayerInRange && !hasTalked && Input.GetKeyDown(KeyCode.LeftAlt))
        {
            hasTalked = true;

            if (hintUIObject != null)
                hintUIObject.SetActive(false);

            // 여기서 대화 시작 코드 호출 가능
            // e.g., npcDialogue.StartConversation();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTalked) return;

        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (hintUIObject != null)
                hintUIObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (hasTalked) return;

        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (hintUIObject != null)
                hintUIObject.SetActive(false);
        }
    }
}
