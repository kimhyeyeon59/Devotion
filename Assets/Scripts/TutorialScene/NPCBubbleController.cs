using UnityEngine;
using System.Collections;

public class NPCBubbleController : MonoBehaviour
{
    public GameObject bubbleUI;
    public float showDuration = 5f;
    public float hideDuration = 2f;

    private Coroutine bubbleCoroutine;
    private bool hasInteracted = false;

    private void Start()
    {
        bubbleCoroutine = StartCoroutine(BubbleLoop());
    }

    private IEnumerator BubbleLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(showDuration);

            if (hasInteracted) yield break; // 상호작용 시 즉시 종료
            bubbleUI.SetActive(false);

            yield return new WaitForSeconds(hideDuration);

            if (hasInteracted) yield break; // 상호작용 시 즉시 종료
            bubbleUI.SetActive(true);
        }
    }

    // 외부에서 호출할 함수
    public void OnPlayerInteracted()
    {
        Debug.Log("NPC와 상호작용했음");

        hasInteracted = true;

        if (bubbleCoroutine != null)
            StopCoroutine(bubbleCoroutine);

        bubbleUI.SetActive(false);
    }

}
