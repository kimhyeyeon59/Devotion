using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueData dialogueData; // Inspector에서 연결

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            ShowDialogue();
        }
    }

    private void ShowDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogueData);
    }
}
