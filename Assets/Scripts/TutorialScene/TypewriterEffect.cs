using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public float charDelay = 0.05f;
    public AudioSource audioSource;
    public AudioClip typingSound;

    public bool IsTyping { get; private set; }
    private Coroutine typingCoroutine;

    public void StartTyping(string message, System.Action onComplete)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(message, onComplete));
    }

    public void Skip()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
            IsTyping = false;
        }
    }

    private IEnumerator TypeText(string message, System.Action onComplete)
    {
        IsTyping = true;
        dialogueText.text = "";

        foreach (char c in message)
        {
            dialogueText.text += c;

            if (!char.IsWhiteSpace(c) && typingSound && audioSource)
                audioSource.PlayOneShot(typingSound, 0.4f);

            yield return new WaitForSeconds(charDelay);
        }

        IsTyping = false;
        onComplete?.Invoke();
    }
}
