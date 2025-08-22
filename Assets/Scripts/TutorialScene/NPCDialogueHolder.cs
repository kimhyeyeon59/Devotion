using UnityEngine;

public class NPCDialogueHolder : MonoBehaviour
{
    public DialogueData firstDialogueData;
    public DialogueData repeatDialogueData;

    private bool hasTalked = false;

    public DialogueData GetDialogueData()
    {
        if (!hasTalked)
        {
            return firstDialogueData;
        }
        else
        {
            return repeatDialogueData != null ? repeatDialogueData : firstDialogueData;
        }
    }

    public void MarkAsTalked()
    {
        hasTalked = true;
    }
}
